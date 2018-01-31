#region
#endregion

namespace gameserver.realm.entity
{
    public class NPC : Character
    {
        public NPC(
            RealmManager manager,
            ushort objectType
            ) : base(manager, objectType, new wRandom())
        {
            log4net.Warn("New NPC registered!");
        }
    }
}
