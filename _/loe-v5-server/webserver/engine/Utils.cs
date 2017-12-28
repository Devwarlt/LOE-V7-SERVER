using System.Linq;
using System.Net.NetworkInformation;

namespace webserver.engine
{
    public class Utils
    {
        public static bool PortCheck(int port) => IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().All(_ => _.LocalEndPoint.Port != port) && IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().All(_ => _.Port != port);

        public static string ProcessFile(string path) => $"_{path}_only.bat";
    }
}
