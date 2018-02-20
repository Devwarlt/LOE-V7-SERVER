#region

using System;
using System.Collections.Generic;
using common.models;
using gameserver.networking.outgoing;
using gameserver.realm.entity;
using gameserver.realm.entity.player;
using gameserver.realm.terrain;

#endregion

namespace gameserver.realm.world
{
    public class Arena : World
    {
        private bool ready = true;
        private bool waiting = false;
        public int wave = 1;
        private List<string> entities = new List<string>();
        private List<string> past_entities = new List<string>();

        public Arena()
        {
            Id = (int)WorldID.ARENA;
            Name = "Arena";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init() => LoadMap("arena", MapType.Wmap);

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            if (Players.Count == 0)
                return;

            CheckOutOfBounds();

            InitArena(time);
        }

        private void InitArena(RealmTime time)
        {
            if (IsEmpty())
            {
                if (ready)
                {
                    if (waiting)
                        return;

                    ready = false;

                    foreach (KeyValuePair<int, Player> i in Players)
                    {
                        if (i.Value.client == null)
                            continue;
                        i.Value.client.SendMessage(new IMMINENT_ARENA_WAVE
                        {
                            CurrentRuntime = time.ElapsedMsDelta,
                            Wave = wave
                        });
                    }

                    waiting = true;

                    Timers.Add(new WorldTimer(5000, (world, t) =>
                    {
                        ready = false;
                        waiting = false;
                        PopulateArena();
                    }));

                    wave++;
                }

                ready = true;
            }
        }

        private void PopulateArena()
        {
            try
            {
                Random rng = new Random();
                if ((wave % 2 == 0) && (wave < 10))
                    for (int i = 0; i < wave / 2; i++)
                        entities.Add(EntityWeak[rng.Next(EntityWeak.Count - 1)]);
                if (wave % 3 == 0)
                    for (int i = 0; i < wave / 3; i++)
                        entities.Add(EntityNormal[rng.Next(EntityNormal.Count - 1)]);
                if ((wave % 2 == 0) && (wave >= 10))
                    for (int i = 0; i < wave / 4; i++)
                        entities.Add(EntityGod[rng.Next(EntityGod.Count - 1)]);
                if ((wave % 5 == 0) && (wave >= 20))
                    entities.Add(EntityQuest[rng.Next(EntityQuest.Count - 1)]);
                if ((entities.Count == 0))
                    entities = past_entities;
                else
                    past_entities = entities;
                foreach (string entity in entities)
                {
                    Position position = new Position { X = rng.Next(12, Map.Width) - 6, Y = rng.Next(12, Map.Height) - 6 };
                    Entity enemy = Entity.Resolve(Program.Manager.GameData.IdToObjectType[entity]);

                    enemy.Move(position.X, position.Y);
                    EnterWorld(enemy);
                }

                entities.Clear();
            }
            catch (Exception e)
            {
                Log.Error($"Arena error found! Error: {e}");
            }
        }

        private int IsEmpty2()
        {
            int quantity = Enemies.Count;

            foreach (Enemy enemy in Enemies.Values)
                if (enemy.IsPet)
                    quantity--;
            return quantity;
        }

        private bool IsEmpty() => IsEmpty2() == 0;

        public bool OutOfBounds(float x, float y) => (Map.Height >= y && Map.Width >= x && x > -1 && y > 0) ? Map[(int)x, (int)y].Region == TileRegion.Outside_Arena : true;

        private void CheckOutOfBounds()
        {
            foreach (KeyValuePair<int, Enemy> i in Enemies)
                if (OutOfBounds(i.Value.X, i.Value.Y))
                    LeaveWorld(i.Value);
        }

        #region "Entities"

        private readonly List<string> EntityWeak = new List<string>
        {
            "Flamer King",
            "Lair Skeleton King",
            "Native Fire Sprite",
            "Native Ice Sprite",
            "Native Magic Sprite",
            "Nomadic Shaman",
            "Ogre King",
            "Orc King",
            "Red Spider",
            "Sand Phantom",
            "Swarm",
            "Tawny Warg",
            "Vampire Bat",
            "Wasp Queen",
            "Weretiger"
        };

        private readonly List<string> EntityNormal = new List<string>
        {
            "Aberrant of Oryx",
            "Abomination of Oryx",
            "Adult White Dragon",
            "Assassin of Oryx",
            //"Bile of Oryx",
            "Gigacorn",
            "Great Lizard",
            "Minotaur",
            "Monstrosity of Oryx",
            "Phoenix Reborn",
            "Shambling Sludge",
            "Urgle"
        };

        private readonly List<string> EntityGod = new List<string>
        {
            "Beholder",
            "Ent God",
            "Flying Brain",
            "Djinn",
            "Ghost God",
            "Leviathan",
            "Medusa",
            "Slime God",
            "Sprite God",
            "White Demon"
        };

        private readonly List<string> EntityQuest = new List<string>
        {
            "Tomb Defender",
            "Tomb Attacker",
            "Tomb Support",
            "Arachna the Spider Queen",
            "Archdemon Malphas",
            "Crystal Prisoner",
            "Grand Sphinx",
            "Limon the Sprite God",
            "Lord Ruthven",
            "Oryx the Mad God 1",
            "Septavius the Ghost God",
            "Stheno the Snake Queen",
            "Thessal the Mermaid Goddess"
        };

        #endregion "Entities"
    }
}