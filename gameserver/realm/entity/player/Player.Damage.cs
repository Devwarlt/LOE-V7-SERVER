#region

using System;
using LoESoft.GameServer.networking.outgoing;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        public void ForceHit(int dmg, Entity chr, bool NoDef)
        {
            if (chr != null)
                Damage(dmg, chr, NoDef);
        }

        public void Damage(int dmg, Entity chr, bool NoDef, bool manaDrain = false)
        {
            if (manaDrain)
            {
                try
                {
                    if (HasConditionEffect(ConditionEffectIndex.Paused))
                        return;

                    MP -= dmg;

                    UpdateCount++;

                    Owner.BroadcastPacket(new NOTIFICATION
                    {
                        ObjectId = Id,
                        Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"-" + dmg + " MP\"}}",
                        Color = new ARGB(0x9B30FF)
                    }, null);

                    SaveToCharacter();
                }
                catch (Exception) { }

            }
            else
            {
                try
                {
                    if (HasConditionEffect(ConditionEffectIndex.Paused) ||
                        HasConditionEffect(ConditionEffectIndex.Stasis) ||
                        HasConditionEffect(ConditionEffectIndex.Invincible))
                        return;

                    dmg = (int)StatsManager.GetDefenseDamage(dmg, NoDef);
                    if (!HasConditionEffect(ConditionEffectIndex.Invulnerable))
                        HP -= dmg;
                    UpdateCount++;
                    Owner.BroadcastPacket(new DAMAGE
                    {
                        TargetId = Id,
                        Effects = 0,
                        Damage = (ushort)dmg,
                        Killed = HP <= 0 || dmg >= HP,
                        BulletId = 0,
                        ObjectId = chr.Id
                    }, this);
                    SaveToCharacter();

                    if (HP <= 0 || dmg >= HP)
                    {
                        HP = 0;
                        Death(chr.ObjectDesc.DisplayId, chr.ObjectDesc);
                    }
                }
                catch (Exception) { }
            }
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                HasConditionEffect(ConditionEffectIndex.Paused) ||
                HasConditionEffect(ConditionEffectIndex.Stasis) ||
                HasConditionEffect(ConditionEffectIndex.Invincible))
                return false;

            return base.HitByProjectile(projectile, time);
        }
    }
}
