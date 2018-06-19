#region

using System.Text;

#endregion

namespace LoESoft.AppEngine
{
    internal class crossdomain : RequestHandler
    {
        protected override void HandleRequest()
        {
            byte[] status = Encoding.UTF8.GetBytes(
                @"<cross-domain-policy>" +
                @"<allow-access-from domain=""*"" to-ports=""*"" />" +
                @"</cross-domain-policy>"
                );
            Context.Response.ContentType = "text/*";
            Context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}