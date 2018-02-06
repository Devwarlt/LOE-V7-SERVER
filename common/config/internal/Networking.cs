using System;
using System.Collections.Generic;

namespace common.config
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
            public static readonly bool DISABLE_NAGLES_ALGORITHM = IS_PRODUCTION;

            public static class RESTART
            {
                public static bool ENABLE_RESTART = Settings.ENABLE_RESTART;
                public static int RESTART_DELAY_MINUTES = Settings.RESTART_DELAY_MINUTES;
            }

            public static readonly List<Tuple<string, bool>> GAME_VERSIONS = new List<Tuple<string, bool>>
            {
                Tuple.Create("v6-1.0", false),
                Tuple.Create("v6-1.1", false),
                Tuple.Create("v6-1.2", false),
                Tuple.Create("v6-1.3", false),
                Tuple.Create("v6-1.4", false),
                Tuple.Create("v6-1.5", false),
                Tuple.Create("v6-1.5.1", false),
                Tuple.Create("v6-1.6", true)
            };

            public static List<Tuple<string, bool>> SUPPORTED_VERSIONS()
            {
                List<Tuple<string, bool>> supportedVersions = new List<Tuple<string, bool>>();
                for (int i = GAME_VERSIONS.Count - 1; i > GAME_VERSIONS.Count - 6; i--)
                    supportedVersions.Add(GAME_VERSIONS[i]);
                return supportedVersions;
            }

            public static string SUPPORTED_VERSIONS_DISPLAY()
            {
                List<string> data = new List<string>();
                foreach (Tuple<string, bool> i in SUPPORTED_VERSIONS())
                    data.Add($"Version: {i.Item1}\t\t(access: {i.Item2})");
                string lastData = data[data.Count - 1];
                data.Remove(data[data.Count - 1]);
                return $"{string.Join("\n\t * ", data.ToArray())}\n\t * {lastData}";
            }

            public static class INTERNAL
            {
                public static readonly List<string> PRODUCTION_DDNS = new List<string>{
                    "testing.loesoft.org", "localhost"
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