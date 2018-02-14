#region

using System.Collections.Generic;

#endregion

namespace gameserver.realm.entity
{
    partial class OneWayContainer : GameObject, IContainer
    {
        public OneWayContainer(ushort objType, int? life, bool dying)
            : base(objType, life, false, dying, false)
        {
            Inventory = new Item[8];
            SlotTypes = new int[8];

            for (int i = 0; i < SlotTypes.Length; i++)
                if (SlotTypes[i] == 0) SlotTypes[i] = 10;
        }

        public int[] SlotTypes { get; private set; }
        public Item[] Inventory { get; private set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.INVENTORY_0_STAT] = (Inventory[0] != null ? Inventory[0].ObjectType : -1);
            stats[StatsType.INVENTORY_1_STAT] = (Inventory[1] != null ? Inventory[1].ObjectType : -1);
            stats[StatsType.INVENTORY_2_STAT] = (Inventory[2] != null ? Inventory[2].ObjectType : -1);
            stats[StatsType.INVENTORY_3_STAT] = (Inventory[3] != null ? Inventory[3].ObjectType : -1);
            stats[StatsType.INVENTORY_4_STAT] = (Inventory[4] != null ? Inventory[4].ObjectType : -1);
            stats[StatsType.INVENTORY_5_STAT] = (Inventory[5] != null ? Inventory[5].ObjectType : -1);
            stats[StatsType.INVENTORY_6_STAT] = (Inventory[6] != null ? Inventory[6].ObjectType : -1);
            stats[StatsType.INVENTORY_7_STAT] = (Inventory[7] != null ? Inventory[7].ObjectType : -1);
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            bool hasItem = false;
            foreach (Item i in Inventory)
                if (i != null)
                {
                    hasItem = true;
                    break;
                }

            if (!hasItem)
            {
                GameObject obj = new GameObject(0x0743, null, false, false, false);
                obj.Move(X, Y);
                World w = Owner;
                Owner.LeaveWorld(this);
                w.EnterWorld(obj);
            }
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }
    }
}