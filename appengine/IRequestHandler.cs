#region

using System.Net;

#endregion

namespace LoESoft.AppEngine
{
    internal interface IRequestHandler
    {
        void HandleRequest(HttpListenerContext context);
    }
}