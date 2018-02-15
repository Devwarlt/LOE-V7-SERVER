#region

using gameserver.networking.incoming;
using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.networking.handlers
{
    internal class PlayerHitHandler : MessageHandlers<PLAYERHIT>
    {
        public override MessageID ID => MessageID.PLAYERHIT;

        protected override void HandleMessage(Client client, PLAYERHIT message) => Handle(client.Player, message);

        private void Handle(Player player, PLAYERHIT message)
        {
            if (player == null)
                return;

            Entity entity = player.Owner.GetEntity(message.ObjectId);

            if (entity == null)
                return;

            Projectile prj = (entity as IProjectileOwner).Projectiles[message.BulletId];

            if (prj == null)
                return;

            entity.Owner.AddProjectileFromId(player.Id, message.BulletId, prj);

            if (prj.ProjDesc.Effects.Length != 0)
                foreach (ConditionEffect effect in prj.ProjDesc.Effects)
                    if (effect.Target == 1)
                        continue;
                    else
                        player.ApplyConditionEffect(effect);

            prj.ForcePlayerHit(player, entity, Manager.Logic.CurrentTime);
        }
    }
}
