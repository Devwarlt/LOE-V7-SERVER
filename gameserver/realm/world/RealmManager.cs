#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using common;
using log4net;
using gameserver.logic;
using gameserver.networking;
using gameserver.realm.commands;
using gameserver.realm.entity.player;
using gameserver.realm.world;
using common.config;
using gameserver.realm.entity.merchant;
using static gameserver.networking.Client;
using gameserver.realm.entity.npc;

#endregion

namespace gameserver.realm
{
    public class RealmManager
    {
        public static List<string> CurrentRealmNames = new List<string>();
        public static List<string> Realms = new List<string>
        {
            "Djinn",
            "Medusa",
            "Beholder",
        };

        public const int MAX_REALM_PLAYERS = 85;

        public ConcurrentDictionary<string, ClientData> ClientManager { get; private set; }
        public ConcurrentDictionary<int, World> Worlds { get; private set; }
        public ConcurrentDictionary<string, World> LastWorld { get; private set; }
        public Random Random { get; }
        public BehaviorDb Behaviors { get; private set; }
        public ChatManager Chat { get; private set; }
        public ISManager InterServer { get; private set; }
        public CommandManager Commands { get; private set; }
        public EmbeddedData GameData { get; private set; }
        public string InstanceId { get; private set; }
        public LogicTicker Logic { get; private set; }
        public int MaxClients { get; private set; }
        public RealmPortalMonitor Monitor { get; private set; }
        public NetworkTicker Network { get; private set; }
        public Database Database { get; private set; }
        public bool Terminating { get; private set; }
        public int TPS { get; private set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(RealmManager));

        private ConcurrentDictionary<string, Vault> vaults { get; set; }

#pragma warning disable CS0649 // Field 'RealmManager.logic' is never assigned to, and will always have its default value null
        private Thread logic;
#pragma warning restore CS0649 // Field 'RealmManager.logic' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'RealmManager.network' is never assigned to, and will always have its default value null
        private Thread network;
#pragma warning restore CS0649 // Field 'RealmManager.network' is never assigned to, and will always have its default value null
        private int nextWorldId;

        public RealmManager(Database db)
        {
            MaxClients = Settings.NETWORKING.MAX_CONNECTIONS;
            TPS = Settings.GAMESERVER.TICKETS_PER_SECOND;
            ClientManager = new ConcurrentDictionary<string, ClientData>();
            Worlds = new ConcurrentDictionary<int, World>();
            LastWorld = new ConcurrentDictionary<string, World>();
            vaults = new ConcurrentDictionary<string, Vault>();
            Random = new Random();
            Database = db;
        }

        #region "Initialize, Run and Stop"

        public void Initialize()
        {
            log.Info("Initializing Realm Manager...");

            GameData = new EmbeddedData();

            //LootSerialization.PopulateLoot();

            Behaviors = new BehaviorDb(this);
            Merchant.InitMerchatLists(GameData);

            AddWorld(World.NEXUS_ID, Worlds[0] = new Nexus());
            AddWorld(World.MARKET, new ClothBazaar());
            AddWorld(World.TEST_ID, new Test());
            AddWorld(World.TUT_ID, new Tutorial(true));
            AddWorld(World.DAILY_QUEST_ID, new DailyQuestRoom());
            Monitor = new RealmPortalMonitor(this);

            Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => AddWorld(_.Result), TaskScheduler.Default);

            InterServer = new ISManager(this);

            Chat = new ChatManager(this);

            Commands = new CommandManager(this);

            log.Info("Realm Manager initialized.");

            log.Info("Initializing NPC Database...");

            NPCs npcs = new NPCs();
            npcs.Initialize(this);

            int j = 1;

            foreach (KeyValuePair<string, NPC> i in NPCs.Database)
            {
                log.InfoFormat("Loading NPC Engine for '{0}' ({1}/{2})...", i.Key, j, NPCs.Database.Count);
                j++;
            }

            log.Info("NPC Database initialized...");
        }

        public void Run()
        {
            log.Info("Starting Realm Manager...");

            Logic = new LogicTicker(this);
            var logic = new Task(() => Logic.TickLoop(), TaskCreationOptions.LongRunning);
            logic.ContinueWith(Program.Stop, TaskContinuationOptions.OnlyOnFaulted);
            logic.Start();

            Network = new NetworkTicker(this);
            var network = new Task(() => Network.TickLoop(), TaskCreationOptions.LongRunning);
            network.ContinueWith(Program.Stop, TaskContinuationOptions.OnlyOnFaulted);
            network.Start();

            log.Info("Realm Manager started.");
        }

        public void Stop()
        {
            log.Info("Stopping Realm Manager...");

            Terminating = true;
            List<Client> saveAccountUnlock = new List<Client>();
            foreach (ClientData cData in ClientManager.Values)
            {
                saveAccountUnlock.Add(cData.client);
                TryDisconnect(cData.client, DisconnectReason.STOPPING_REALM_MANAGER);
            }

            GameData?.Dispose();
            logic?.Join();
            network?.Join();

            log.Info("Realm Manager stopped.");
        }

        #endregion

        #region "Connection handlers"

        /** Disconnect Handler (LoESoft Games)
	    * Author: DV
	    * Original Idea: Miniguy
	    */

        public ConnectionProtocol TryConnect(Client client)
        {
            try
            {
                ClientData _cData = new ClientData();
                _cData.ID = client.Account.AccountId;
                _cData.client = client;
                _cData.DNS = client.Socket.RemoteEndPoint.ToString().Split(':')[0];
                _cData.registered = DateTime.Now;

                if (ClientManager.Count >= MaxClients) // When server is full.
                    return new ConnectionProtocol(false, ErrorIDs.SERVER_FULL);

                if (ClientManager.ContainsKey(_cData.ID))
                {
                    if (_cData.client != null)
                    {
                        TryDisconnect(ClientManager[_cData.ID].client); // Old client.

                        return new ConnectionProtocol(ClientManager.TryAdd(_cData.ID, _cData), ErrorIDs.NORMAL_CONNECTION); // Normal connection with reconnect type.
                    }

                    return new ConnectionProtocol(false, ErrorIDs.LOST_CONNECTION); // User dropped connection while reconnect.
                }

                return new ConnectionProtocol(ClientManager.TryAdd(_cData.ID, _cData), ErrorIDs.NORMAL_CONNECTION); // Normal connection with reconnect type.
            }
            catch (Exception e)
            {
                Log.Write($"An error occurred.\n{e}", ConsoleColor.Red);
            }

            return new ConnectionProtocol(false, ErrorIDs.LOST_CONNECTION); // User dropped connection while reconnect.
        }

        public void TryDisconnect(Client client, DisconnectReason reason = DisconnectReason.UNKNOW_ERROR_INSTANCE)
        {
            if (client == null)
                return;
            DisconnectHandler(client, reason == DisconnectReason.UNKNOW_ERROR_INSTANCE ? DisconnectReason.REALM_MANAGER_DISCONNECT : reason);
        }

        public void DisconnectHandler(Client client, DisconnectReason reason)
        {
            try
            {
                if (ClientManager.ContainsKey(client.Account.AccountId))
                {
                    ClientData _disposableCData;

                    ClientManager.TryRemove(client.Account.AccountId, out _disposableCData);

                    Log.Write($"[({(int)reason}) {reason.ToString()}] Disconnect player '{_disposableCData.client.Account.Name} (Account ID: {_disposableCData.client.Account.AccountId})'.");

                    _disposableCData.client.Save();
                    _disposableCData.client.State = ProtocolState.Disconnected;
                    _disposableCData.client.Socket.Close();
                    _disposableCData.client.Dispose();
                }
                else
                {
                    Log.Write($"[({(int)reason}) {reason.ToString()}] Disconnect player '{client.Account.Name} (Account ID: {client.Account.AccountId})'.");

                    client.Save();
                    client.State = ProtocolState.Disconnected;
                    client.Dispose();
                }
            }
            catch (NullReferenceException) { }
        }

        #endregion

        #region "World Utils"

        public World AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = id;
            Worlds[id] = world;
            OnWorldAdded(world);
            return world;
        }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = Interlocked.Increment(ref nextWorldId);
            Worlds[world.Id] = world;
            OnWorldAdded(world);
            return world;
        }

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");
            World dummy;
            if (Worlds.TryRemove(world.Id, out dummy))
            {
                try
                {
                    OnWorldRemoved(world);
                    world.Dispose();
                    GC.Collect();
                }
                catch (Exception e)
                { log.Fatal(e); }
                return true;
            }
            return false;
        }

        public void CloseWorld(World world)
        {
            Monitor.WorldRemoved(world);
        }

        public World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        public bool RemoveVault(string accountId)
        {
            Vault dummy;
            return vaults.TryRemove(accountId, out dummy);
        }

        private void OnWorldAdded(World world)
        {
            if (world.Manager == null)
                world.Manager = this;
            if (world is GameWorld)
                Monitor.WorldAdded(world);
            log.InfoFormat("World {0}({1}) added.", world.Id, world.Name);
        }

        private void OnWorldRemoved(World world)
        {
            world.Manager = null;
            if (world is GameWorld)
                Monitor.WorldRemoved(world);
            log.InfoFormat("World {0}({1}) removed.", world.Id, world.Name);
        }

        #endregion

        #region "Player Utils"

        public Player FindPlayer(string name)
        {
            if (name.Split(' ').Length > 1)
                name = name.Split(' ')[1];

            return (from i in Worlds
                    where i.Key != 0
                    from e in i.Value.Players
                    where string.Equals(e.Value.client.Account.Name, name, StringComparison.CurrentCultureIgnoreCase)
                    select e.Value).FirstOrDefault();
        }

        public Player FindPlayerRough(string name)
        {
            Player dummy;
            foreach (KeyValuePair<int, World> i in Worlds)
                if (i.Key != 0)
                    if ((dummy = i.Value.GetUniqueNamedPlayerRough(name)) != null)
                        return dummy;
            return null;
        }

        public Vault PlayerVault(Client processor)
        {
            Vault v;
            if (!vaults.TryGetValue(processor.Account.AccountId, out v))
                vaults.TryAdd(processor.Account.AccountId, v = (Vault)AddWorld(new Vault(false, processor)));
            else
                v.Reload(processor);
            return v;
        }

        #endregion
    }

    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Networking,
        Normal,
        Creation,
    }

    public struct RealmTime
    {
        public long TickCount { get; set; }
        public long TotalElapsedMs { get; set; }
        public int TickDelta { get; set; }
        public int ElapsedMsDelta { get; set; }
    }

    public class TimeEventArgs : EventArgs
    {
        public TimeEventArgs(RealmTime time)
        {
            Time = time;
        }

        public RealmTime Time { get; private set; }
    }

    public class ConnectionProtocol
    {
        public bool connected { get; private set; }
        public ErrorIDs errorID { get; private set; }

        public ConnectionProtocol(
            bool connected,
            ErrorIDs errorID
            )
        {
            this.connected = connected;
            this.errorID = errorID;
        }
    }

    public class ClientData
    {
        public string ID { get; set; }
        public Client client { get; set; }
        public string DNS { get; set; }
        public DateTime registered { get; set; }
    }
}