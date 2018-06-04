using System.Collections.Generic;

namespace LoESoft.Core.config
{
    public partial class Settings
    {
        public static class NETWORKING
        {
            public static readonly byte[] INCOMING_CIPHER = new byte[] { 0x3D, 0xC1, 0xC4, 0x44, 0xF5, 0x78, 0xC1, 0xEC, 0x7B, 0xF4, 0x0A, 0x4D, 0xCA, 0x94, 0x93, 0xA2 };
            public static readonly byte[] OUTGOING_CIPHER = new byte[] { 0x78, 0x9A, 0x63, 0x2F, 0x43, 0xA2, 0xF5, 0x5C, 0xB0, 0xA4, 0xC3, 0x99, 0x9C, 0x32, 0x4D, 0xA0 };
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

                /// <summary>
                /// Use program "crossdomain.exe" to generate correct crossdomain template
                /// </summary>
                public static readonly string SELECTED_DOMAINS =
                    @"<cross-domain-policy>
                        <policy-file-request/>
                        <site-control permitted-cross-domain-policies=""master-only""/>
                        <allow-access-from domain=""loesoft-games.github.io"" secure=""true""/>
                        <allow-access-from domain=""loesoft-games.github.io"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""loesoft-games.github.io"" headers=""*"" secure=""true""/>
                        <allow-access-from domain=""loesoft.org"" secure=""false""/>
                        <allow-access-from domain=""loesoft.org"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""loesoft.org"" headers=""*"" secure=""false""/>
                        <allow-access-from domain=""testing.loesoft.org"" secure=""false""/>
                        <allow-access-from domain=""testing.loesoft.org"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""testing.loesoft.org"" headers=""*"" secure=""false""/>
                        <allow-access-from domain=""appengine.loesoft.org"" secure=""false""/>
                        <allow-access-from domain=""appengine.loesoft.org"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""appengine.loesoft.org"" headers=""*"" secure=""false""/>
                    </cross-domain-policy>";

                public static readonly string LOCALHOST_DOMAINS =
                    @"<cross-domain-policy>
                        <policy-file-request/>
                        <allow-access-from domain=""*""/>
                    </cross-domain-policy>";
            }
        }
    }
}