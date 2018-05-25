using System.Collections.Generic;
using System.Linq;

namespace LoESoft.Core.config
{
    public partial class Settings
    {
        public enum Vocations : int
        {
            APPRENTICE = 0,
            PALADIN = 1,
            HUNTER = 2,
            WARLOCK = 3
        }

        public static bool IS_PRODUCTION = true;

        public static bool ENABLE_RESTART = true;

        public static int RESTART_DELAY_MINUTES = 60;

        public static class NewCharacter
        {
            public static int VocationType
                => (int)Vocations.APPRENTICE;
            public static int InitialLevel
                => 1;
            public static double InitialExperience
                => 0;
            public static int InitialHealthPoints
                => 100;
            public static int InitialMagicPoints
                => 0;
            public static int InitialSpeedBase
                => 10;
            public static int InitialAttackLevel
                => 0;
            public static double InitialAttackExperience
                => 0;
            public static int InitialDefenseLevel
                => 0;
            public static double InitialDefenseExperience
                => 0;
            public static int InitialOutfit
                => 0;
        }

        public class Vocation
        {
            public static int AttackSpeed
                => 20;

            public static int SpeedBaseIncrement
                => 1;

            public int HPRate
            { get; private set; }

            public int MPRate
            { get; private set; }

            public Healing HPRegen
            { get; private set; }

            public Healing MPRegen
            { get; private set; }

            public double AttRate
            { get; private set; }

            public double DefRate
            { get; private set; }

            public List<int> Outfits
            { get; private set; }

            public Vocation(
                int HPRate,
                int MPRate,
                Healing HPRegen,
                Healing MPRegen,
                double AttRate,
                double DefRate,
                int[] Outfits
                )
            {
                this.HPRate = HPRate;
                this.MPRate = MPRate;
                this.HPRegen = HPRegen;
                this.MPRegen = MPRegen;
                this.AttRate = AttRate;
                this.DefRate = DefRate;
                this.Outfits = Outfits.ToList();
            }

            public bool AchievedLastedOutfit(int level)
                => (level / 50) >= Outfits.Count;

            public int GetLatestOutfit
                => Outfits[Outfits.Count - 1];
        }

        public class Healing
        {
            public int Amount
            { get; private set; }

            public int Cooldown
            { get; private set; }

            public Healing(
                int Amount,
                int Cooldown
                )
            {
                this.Amount = Amount;
                this.Cooldown = Cooldown;
            }
        }

        public static Dictionary<Vocations, Vocation> VocationCache = new Dictionary<Vocations, Vocation>
        {
            { Vocations.APPRENTICE, new Vocation(HPRate: 5, MPRate: 5, HPRegen: new Healing(Amount: 1, Cooldown: 3000), MPRegen: new Healing(Amount: 1, Cooldown: 5000), AttRate: 0.1, DefRate: 0.1, Outfits: new int[] { 0, 0, 0, 0, 0, 0 } ) },
            { Vocations.PALADIN, new Vocation(HPRate: 15, MPRate: 5, HPRegen: new Healing(Amount: 2, Cooldown: 3000), MPRegen: new Healing(Amount: 1, Cooldown: 5000), AttRate: 1.0, DefRate: 1.0, Outfits: new int[] { 0, 0, 0, 0, 0, 0 } ) },
            { Vocations.HUNTER, new Vocation(HPRate: 10, MPRate: 10, HPRegen: new Healing(Amount: 1, Cooldown: 4000), MPRegen: new Healing(Amount: 2, Cooldown: 4000), AttRate: 1.0, DefRate: 0.5, Outfits: new int[] { 0, 0, 0, 0, 0, 0 } ) },
            { Vocations.WARLOCK, new Vocation(HPRate: 5, MPRate: 30, HPRegen: new Healing(Amount: 1, Cooldown: 5000), MPRegen: new Healing(Amount: 2, Cooldown: 3000), AttRate: 0.5, DefRate: 0.3, Outfits: new int[] { 0, 0, 0, 0, 0, 0 } ) }
        };

        public static class STARTUP
        {
            public static readonly int GOLD = 999999999;
            public static readonly int EMPIRES_COIN = 999999999;
            public static readonly int MAX_CHAR_SLOTS = 2;
            public static readonly int IS_AGE_VERIFIED = 1;
            public static readonly bool VERIFIED = true;
        }

        public static readonly List<GameVersion> GAME_VERSIONS = new List<GameVersion>
        {
            new GameVersion(Version: "1.0", Allowed: false),
            new GameVersion(Version: "1.1", Allowed: false),
            new GameVersion(Version: "1.2", Allowed: false),
            new GameVersion(Version: "1.3", Allowed: false),
            new GameVersion(Version: "1.4", Allowed: false),
            new GameVersion(Version: "1.5", Allowed: false),
            new GameVersion(Version: "1.5.1", Allowed: false),
            new GameVersion(Version: "1.6 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.1 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.2 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.3 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.4 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.5 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.6 edition 1: pre-beta", Allowed: false),
            new GameVersion(Version: "1.6.7 edition 1: pre-beta", Allowed: true),
            new GameVersion(Version: "1.6.8 edition 1: pre-beta", Allowed: true)
        };
    }
}