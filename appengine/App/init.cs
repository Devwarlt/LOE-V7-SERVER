#region

using common.config;
using System.IO;
using System.Net;

#endregion

namespace appengine.app
{
    internal class init : RequestHandler
    {
        protected override void HandleRequest() => WriteLine(File.ReadAllText("app/init.xml"));
    }
}