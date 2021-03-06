﻿using System;
using System.Collections.Generic;

namespace LoESoft.Core.config
{
    public partial class Settings
    {
        [Flags]
        public enum ServerMode
        {
            Local,
            ClosedTest,
            Production
        }

        public static readonly ServerMode SERVER_MODE = ServerMode.Local;

        public static readonly bool ENABLE_RESTART_SYSTEM = false;

        public static readonly int RESTART_APPENGINE_DELAY_MINUTES = 4 * 60;

        public static readonly int RESTART_DELAY_MINUTES = 60;

        public static readonly List<GameVersion> GAME_VERSIONS = new List<GameVersion>
        {
            new GameVersion(Version: "0.0.1", Allowed: false),
            new GameVersion(Version: "0.0.2", Allowed: false),
            new GameVersion(Version: "0.0.3", Allowed: false),
            new GameVersion(Version: "0.0.4", Allowed: true)
        };

        public static readonly List<string> ALLOWED_LOCAL_DNS = new List<string>
        {
            "::1", "localhost", "127.0.0.1", "189.61.24.57", "testing.loesoftgames.ignorelist.com"
        };

        public class Beginner
        {
            /*
             * CharLevel
             * CharExperience
             * CharHealthPoints
             * CharMagicPoints
             * CharAttackLevel
             * CharAttackExperience
             * CharDefenseLevel
             * CharDefenseExperience
             * CharSpeed
             * CharPosition
             * CharTownID
             */
            public static ushort Apprentice = 0x0300;
            public static int CharLevel = 1;
            public static string CharExperience = "0";
            public static int CharHealthPoints = 100;
            public static int CharMagicPoints = 0;
            public static int CharAttackLevel = 0;
            public static string CharAttackExperience = "0";
            public static int CharDefenseLevel = 0;
            public static string CharDefenseExperience = "0";
            public static int CharSpeed = 0;
            public static int CharMaxHealthPoints = 100;
            public static int CharMaxMagicPoints = 0;
            public static string CharNextExperience = "100";
            public static string CharNextAttackExperience = "25";
            public static string CharNextDefenseExperience = "25";
            public static string CharPosition = "X:25;Y:51;Town:-1";
            public static int CharTownID = -1;
        }

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
    }
}