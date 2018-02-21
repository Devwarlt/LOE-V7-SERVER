﻿#region

using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.logic.transitions
{
    public class DamageTakenTransition : Transition
    {
        //State storage: none

        private int damage;

        public DamageTakenTransition(int damage, string targetState)
            : base(targetState)
        {
            this.damage = damage;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            int damageSoFar = 0;

            foreach (var i in (host as Enemy).DamageCounter.GetPlayerData())
                damageSoFar += i.Item2;

            if (damageSoFar >= damage)
                return true;
            return false;
        }
    }
}
