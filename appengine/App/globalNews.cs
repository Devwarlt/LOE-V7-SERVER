using System.IO;

namespace appengine.app
{
    internal class globalNews : RequestHandler
    {
        protected override void HandleRequest() => WriteLine(File.ReadAllText("app/globalNews/globalNews.json"), false);
    }
}