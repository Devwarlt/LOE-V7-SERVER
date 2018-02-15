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

                Log.Write($"Receiving new projectile:\n\t- Player: {player.Name}\n\t- Target: {entity.Name}\n\t- Bullet ID: {message.BulletId}\n\t- Killed? {message.Killed}");

                if (prj == null)
                    return;

                Log.Write($"\t- Damage: {prj.Damage}");

                prj.ForceHit(entity, time);
            }
        }
    }
}