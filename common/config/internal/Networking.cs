﻿using System.Collections.Generic;

namespace LoESoft.Core.config
{
    public partial class Settings
    {
        public static class NETWORKING
        {
            public static readonly byte[] INCOMING_CIPHER = ProcessToken("3DC1C444F578C1EC7BF40A4DCA9493A2");
            public static readonly byte[] OUTGOING_CIPHER = ProcessToken("789A632F43A2F55CB0A4C3999C324DA0");
            public static readonly string APPENGINE_URL = "https://loesoft-games.github.io"; //"http://appengine.loesoft.org";
            public static readonly int CPU_HANDLER = 4096;
            public static readonly int MAX_CONNECTIONS = 25;
            public static readonly bool DISABLE_NAGLES_ALGORITHM = SERVER_MODE != ServerMode.Local;

            public static class RESTART
            {
                public static readonly bool ENABLE_RESTART = ENABLE_RESTART_SYSTEM;
                public static readonly int RESTART_DELAY_MINUTES = Settings.RESTART_DELAY_MINUTES;
                public static readonly int RESTART_APPENGINE_DELAY_MINUTES = Settings.RESTART_APPENGINE_DELAY_MINUTES;
            }

            public static class INTERNAL
            {
                public static readonly List<string> PRODUCTION_DDNS = new List<string>{
                    "testing.loesoftgames.ignorelist.com", "localhost"
                };

                public static readonly string CROSS_DOMAIN_POLICY =
                    @"<cross-domain-policy>
                        <policy-file-request/>
                        <site-control permitted-cross-domain-policies=""master-only""/>
                        <allow-access-from domain=""loesoft-games.github.io"" secure=""true""/>
                        <allow-access-from domain=""loesoft-games.github.io"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""loesoft-games.github.io"" headers=""*"" secure=""true""/>
                        <allow-access-from domain=""testing.loesoftgames.ignorelist.com"" secure=""false""/>
                        <allow-access-from domain=""testing.loesoftgames.ignorelist.com"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""testing.loesoftgames.ignorelist.com"" headers=""*"" secure=""true""/>
                        <allow-access-from domain=""189.61.24.57"" secure=""false""/>
                        <allow-access-from domain=""189.61.24.57"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""189.61.24.57"" headers=""*"" secure=""true""/>
                    </cross-domain-policy>";
            }
        }
    }
}