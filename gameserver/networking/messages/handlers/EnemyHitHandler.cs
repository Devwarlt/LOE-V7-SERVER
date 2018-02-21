﻿#region

using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.networking.handlers
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

            if (entity == null)
                return;

            Projectile prj = player.Owner.GetProjectileFromId(player.Id, message.BulletId);

            if (prj == null)
                return;

            prj.Owner.RemoveProjectileFromId(player.Id, message.BulletId);

            if (prj.ProjDesc.Effects.Length != 0)
                foreach (ConditionEffect effect in prj.ProjDesc.Effects)
                    if (effect.Target == 1)
                        continue;
                    else
                        entity.ApplyConditionEffect(effect);

            prj.ForceHit(entity, time, message.Killed);
        }
    }
}