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
            if (player == null)
                return;

            Entity entity = player.Owner.GetEntity(message.TargetId);

            if (entity != null)
            {

                if (message.Killed)
                    player.ClientKilledEntity.Enqueue(entity);

                Projectile prj = entity.Owner.GetProjectileFromId(player.Id, message.BulletId);

                if (prj == null)
                    return;

                prj.ForceHit(entity, time);
            }
        }
    }
}