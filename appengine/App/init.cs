#region

using System.IO;

#endregion

namespace LoESoft.AppEngine.app
{
    internal class init : RequestHandler
    {
        protected override void HandleRequest() => WriteLine(File.ReadAllText("app/init.xml"));
    }
}