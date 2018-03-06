using System.Collections.Generic;

namespace LoESoft.Core.config
{
    public partial class Settings
    {
        public static bool IS_PRODUCTION = false;

        public static bool ENABLE_RESTART = true;

        public static int RESTART_DELAY_MINUTES = 60;

        public static class STARTUP
        {
            public static readonly int GOLD = 999999999;
            public static readonly int FAME = 999999999;
            public static readonly int TOTAL_FAME = 999999999;
            public static readonly int TOKENS = 0;
            public static readonly int EMPIRES_COIN = 999999999;
            public static readonly bool VERIFIED = true;
            public static readonly int MAX_CHAR_SLOTS = 2;
            public static readonly int IS_AGE_VERIFIED = 1;
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
            new GameVersion(Version: "1.6.5 edition 1: pre-beta", Allowed: true),
            new GameVersion(Version: "1.6.6 edition 1: pre-beta", Allowed: true)
        };
    }
}