#region

using common.config;
using System.Net;

#endregion

namespace appengine.app
{
    internal class init : RequestHandler
    {
        protected override void HandleRequest()
        {
            string gameNetwork = Query["game_net"];
            if (gameNetwork != null)
            {
                try
                {
                    WebClient client = new WebClient();
                    string file = Context.Request.Url.LocalPath + ".xml";
                    string appengine = Settings.NETWORKING.APPENGINE_URL;
                    string response = client.DownloadString(appengine + file);
                    client.Dispose();
                    if (response != null)
                        WriteLine(response);
                    else
                        return;
                } catch
                {
                    Program.Logger.Error("Unreachable host, maybe caused by connection lost from user.");
                    return;
                }
            } else
                return;
        }
    }
}