#region

using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class CopyDamageOnDeath : Behavior
    {
        private float dist;
        private string child;

        public CopyDamageOnDeath(
            string child,
            float dist = 50
            )
        {
            this.dist = dist;
            this.child = child;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (e.Host.GetNearestEntity(dist, Program.Manager.GameData.IdToObjectType[child]) is Enemy en)
                {
                    en.SetDamageCounter((e.Host as Enemy).DamageCounter, en);
                }
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
