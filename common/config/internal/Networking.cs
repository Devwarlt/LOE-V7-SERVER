using System;
using System.Collections.Generic;

namespace common.config
{
    public partial class Settings
    {
        public static class NETWORKING
        {
            public static class RESTART
            {
                public static bool ENABLE_RESTART = Settings.ENABLE_RESTART;
                public static int RESTART_DELAY_MINUTES = Settings.RESTART_DELAY_MINUTES;
            }

            public static readonly List<Tuple<string, bool>> GAME_VERSIONS = new List<Tuple<string, bool>>
            {
                // outdated
                Tuple.Create("v6-1.0", false),
                Tuple.Create("v6-1.1", false),
                Tuple.Create("v6-1.2", false),
                Tuple.Create("v6-1.3", false),
                Tuple.Create("v6-1.4", false),
                Tuple.Create("v6-1.5", false),
                Tuple.Create("v6-1.5.1", false),
                Tuple.Create("v6-1.6", true) // new message into HELLO (require new version to play)
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

            public static string APPENGINE_URL = "https://devwarlt.github.io"; //"http://appengine.loesoft.org";
            public static int CPU_HANDLER = 4096;
            public static int MAX_CONNECTIONS = 25;
            public static bool DISABLE_NAGLES_ALGORITHM = IS_PRODUCTION;

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
                        <allow-access-from domain=""devwarlt.github.io"" secure=""true""/>
                        <allow-access-from domain=""devwarlt.github.io"" to-ports=""*""/>
                        <allow-http-request-headers-from domain=""devwarlt.github.io"" headers=""*"" secure=""true""/>
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