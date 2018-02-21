#region

using LoESoft.Core.config;

#endregion

namespace LoESoft.AppEngine.sfx
{
    internal class Sfx : RequestHandler
    {
        protected override void HandleRequest()
        {
            string file = Context.Request.Url.LocalPath;
            if (file.StartsWith("/music") || file.StartsWith("/sfx"))
                Context.Response.Redirect(Settings.NETWORKING.APPENGINE_URL + file);
        }
    }
}