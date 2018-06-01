using System.Collections.Generic;

namespace LoESoft.Core.config
{
    public partial class Settings
    {
        public static bool IS_PRODUCTION = false;

        public static bool ENABLE_RESTART = false;

        public static int RESTART_DELAY_MINUTES = 60;

        public static class STARTUP
        {
            public static readonly int GOLD = 999999999;
            public static readonly int FAME = 999999999;
            public static readonly int TOTAL_FAME = 999999999;
            public static readonly int TOKENS = 0;
            public static readonly int EMPIRES_COIN = 999999999;
            public static readonly int MAX_CHAR_SLOTS = 2;
            public static readonly int IS_AGE_VERIFIED = 1;
            public static readonly bool VERIFIED = true;
        }

        public static readonly List<GameVersion> GAME_VERSIONS = new List<GameVersion>
        {
            new GameVersion(Version: "0.0.1", Allowed: true)
        };
    }
}