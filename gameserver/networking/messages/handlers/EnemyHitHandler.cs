#region

using gameserver.networking.incoming;
using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.networking.handlers
{
    internal class EnemyHitHandler : MessageHandlers<ENEMYHIT>
    {
        public override MessageID ID => MessageID.ENEMYHIT;

        protected override void HandleMessage(Client client, ENEMYHIT message) => Manager.Logic.AddPendingAction(t => Handle(client.Player, t, message));

        private void Handle(Player player, RealmTime time, ENEMYHIT message)
        {
            if (player?.Owner == null) return;

            Entity entity = player.Owner.GetEntity(message.TargetId);

            if (entity != null)
            {
                Projectile prj = (player as IProjectileOwner).Projectiles[message.BulletId];

                prj?.ForceHit(entity, time);

                if (message.Killed)
                    player.ClientKilledEntity.Enqueue(entity);
            }
        }
    }
}