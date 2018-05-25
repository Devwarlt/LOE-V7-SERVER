#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LoESoft.GameServer.logic;
using LoESoft.GameServer.networking;
using LoESoft.Core.config;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        public int MaxHackEntries { get; set; }
        public AccountTypePerks AccountPerks { get; set; }
        public int AccountType { get; set; }
        public DateTime AccountLifetime { get; set; }
        public bool IsVip { get; set; }
        public int Admin { get; set; }
        public const int Radius = 20;
        public const int RadiusSqr = Radius * Radius;
        public readonly ConcurrentQueue<Entity> ClientKilledEntity = new ConcurrentQueue<Entity>();
        private const float MaxTimeDiff = 1.08f;
        private const float MinTimeDiff = 0.92f;
        private readonly TimeCop _time = new TimeCop();
        private int _shotsLeft;
        private int _lastShootTime;
        private readonly ConcurrentQueue<int> _move = new ConcurrentQueue<int>();
        private readonly ConcurrentQueue<int> _clientTimeLog = new ConcurrentQueue<int>();
        private readonly ConcurrentQueue<int> _serverTimeLog = new ConcurrentQueue<int>();
        public int LastClientTime = -1;
        public long LastServerTime = -1;
        private bool dying;
        private Item[] inventory;
        private float hpRegenCounter;
        private float mpRegenCounter;
        private byte[,] tiles;
        private SetTypeSkin setTypeSkin;
        public string AccountId { get; }
        public Client Client { get; }
        public int Credits { get; set; }
        public double Experience { get; set; }
        public double ExperienceGoal { get; set; }
        public TaskManager TaskManager { get; }
        public bool Glowing { get; set; }
        public List<string> Ignored { get; set; }
        public bool Invited { get; set; }
        public bool Muted { get; set; }
        public int Level { get; set; }
        public List<string> Locked { get; set; }
        public int MaxHp { get; set; }
        public int MaxMp { get; set; }
        public int MP { get; set; }
        public bool NameChosen { get; set; }
        public int PlayerSkin { get; set; }
        public int Stars { get; set; }
        public StatsManager StatsManager { get; }
        public Item[] Inventory { get { return inventory; } set { inventory = value; } }
        public string Guild { get; set; }
        public int GuildRank { get; set; }
        public int[] SlotTypes { get; set; }
        private int CanTPCooldownTime;
        private float bleeding;
        private int healCount;
        private float healing;
        private int newbieTime;
        private long b;
        private readonly Random invRand = new Random();
        private int[] setTypeBoosts;
        private int updateLastSeen;
        public Enemy Quest { get; private set; }
        private bool worldBroadcast = true;
        private readonly Queue<Tuple<Message, Predicate<Player>>> pendingMessages = new Queue<Tuple<Message, Predicate<Player>>>();
        public TradeManager HandleTrade { get; private set; }
        public int UpdatesSend { get; private set; }
        public int UpdatesReceived { get; set; }
        public const int SIGHTRADIUS = 15;
        private const int APPOX_AREA_OF_SIGHT = (int)(Math.PI * SIGHTRADIUS * SIGHTRADIUS + 1);
        private readonly HashSet<Entity> clientEntities = new HashSet<Entity>();
        private readonly HashSet<IntPoint> clientStatic = new HashSet<IntPoint>(new IntPointComparer());
        private readonly ConcurrentDictionary<Entity, int> lastUpdate = new ConcurrentDictionary<Entity, int>();
        public Dictionary<IntPoint, bool> visibleTiles;
        private int mapHeight;
        private int mapWidth;
        private int tickId;
        public List<IntPoint> blocksight = new List<IntPoint>();
        public static int Oldstat { get; set; }
        public static Position Targetlink { get; set; }
        private const int PingPeriod = 3000;
        public const int DcThresold = 30000;
        private long _pingTime = -1;
        private long _pongTime = -1;
        private int _cnt;
        private long _sum;
        public long TimeMap { get; private set; }
        private long _latSum;
        public int Latency { get; private set; }
    }
}