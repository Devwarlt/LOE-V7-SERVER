#region

using common;

#endregion

namespace appengine.fame
{
    internal class list : RequestHandler
    {
        // timespan, accountId, charId
        // <Error>Invalid fame list</Error>
        protected override void HandleRequest()
        {
            DbChar character = null;
            if (Query["accountId"] != null)
                character = Database.LoadCharacter(int.Parse(Query["accountId"]), int.Parse(Query["charId"]));
            var list = FameList.FromDb(Database, Query["timespan"], character);
            WriteLine(list.ToXml());
        }
    }
}