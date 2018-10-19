﻿#region

using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;
using Mono.Game;
using System;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class ReturnToSpawn : CycleBehavior
    {
        private readonly float speed;
        private bool once;
        private bool returned;

        public ReturnToSpawn(
            bool once = false,
            double speed = 2
            )
        {
            this.speed = (float)speed / 10;
            this.once = once;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (!returned)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                    return;

                var dist = host.EntitySpeed(speed, time);

                Position pos = (host as Enemy).SpawnPoint;
                var tx = pos.X;
                var ty = pos.Y;
                if (Math.Abs(tx - host.X) > 1 || Math.Abs(ty - host.Y) > 1)
                {
                    var x = host.X;
                    var y = host.Y;
                    Vector2 vect = new Vector2(tx, ty) - new Vector2(host.X, host.Y);
                    vect.Normalize();
                    vect *= dist;
                    host.Move(host.X + vect.X, host.Y + vect.Y);
                    host.UpdateCount++;
                }

                if (host.X == pos.X && host.Y == pos.Y && once)
                {
                    once = true;
                    returned = true;
                }
            }
        }
    }
}