#region

using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.realm.entity
{
    public partial class Portal : GameObject
    {
        public Portal(ushort objType, int? life)
            : base(objType, life, false, true, false)
        {
            Usable = objType != 0x0721;
            ObjectDesc = GameServer.Manager.GameData.Portals[objType];
            Name = GameServer.Manager.GameData.Portals[objType].DisplayId;
        }

        private Portal(PortalDesc desc, int? life)
            : base(desc.ObjectType, life, false, true, false)
        {
            ObjectDesc = desc;
            Name = desc.DisplayId;
        }

        public string PortalName { get; set; }
        public new PortalDesc ObjectDesc { get; }
        public new ushort ObjectType => ObjectDesc.ObjectType;
        public new World WorldInstance { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (ObjectType != 0x072f)
                stats[StatsType.ACTIVE_STAT] = Usable ? 1 : 0;
            base.ExportStats(stats);
            stats[StatsType.NAME_STAT] = ObjectDesc.DungeonName ?? Name;
        }

        public override ObjectDef ToDefinition()
        {
            return new ObjectDef
            {
                ObjectType = ObjectDesc.ObjectType,
                Stats = ExportStats()
            };
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }

        public bool IsRealmPortal
        {
            get { return Owner.Id == -2 && Name.StartsWith("NexusPortal."); }
        }

        public Portal Unlock(string dungeonName)
        {
            var desc = GameServer.Manager.GameData.Portals[0x0700];
            desc.DungeonName = dungeonName;
            var portal = new Portal(desc, desc.TimeoutTime * 1000);
            portal.Move(X, Y);
            portal.Usable = true;
            Owner.EnterWorld(portal);
            Owner.LeaveWorld(this);
            return portal;
        }
    }
}