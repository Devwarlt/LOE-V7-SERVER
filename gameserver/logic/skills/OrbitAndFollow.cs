#region

using LoESoft.Core.models;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity.player;
using Mono.Game;
using System;
using System.Text;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class OrbitAndFollow : CycleBehavior
    {
        private class OrbitAndFollowState
        {
            public double AngularVelocity { get; set; }
            public double Angle { get; set; }
            public double Radius { get; set; }
            public int Clockwise { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 TargetPosition { get; set; }

            public override string ToString()
            {
                var ret = new StringBuilder("{\n");
                var arr = GetType().GetProperties();

                for (var i = 0; i < arr.Length; i++)
                {
                    if (i != 0) ret.Append(",\n");
                    ret.AppendFormat("\t{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
                }
                ret.Append("\n}");

                return ret.ToString();
            }
        }

        private double AngularVelocity { get; } = 5;
        private double Radius { get; }
        private int Clockwise { get; }

        public OrbitAndFollow(double angularVelocity, double radius, bool clockwise)
        {
            AngularVelocity = angularVelocity;
            Radius = radius <= 0 ? 1 : radius;
            Clockwise = !clockwise ? 1 : -1;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state) => state = new OrbitAndFollowState()
        {
            AngularVelocity = AngularVelocity / 10,
            Angle = 0,
            Radius = Radius,
            Clockwise = Clockwise,
            Position = new Vector2()
            {
                X = host.X,
                Y = host.Y
            },
            TargetPosition = new Vector2()
            {
                X = 0,
                Y = 0
            }
        };

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            var s = state as OrbitAndFollowState;
            var entity = host.GetNearestEntity(12, null);

            Status = CycleStatus.NotStarted;

            if (entity != null)
            {
                var hostVelocity = host.EntitySpeed((float)s.AngularVelocity, time);
                var targetVelocity = entity is Player ?
                    (entity as Player).StatsManager.GetSpeed() / 10 :
                    hostVelocity;
                var angle = (host.Y == entity.Y && host.X == entity.X) ?
                    Math.Atan2(host.Y - entity.Y + (Random.NextDouble() * 2 - 1), host.X - entity.X + (Random.NextDouble() * 2 - 1)) :
                    Math.Atan2(host.Y - entity.Y, host.X - entity.X);
                angle += (float)(s.Clockwise * hostVelocity / s.Radius);

                var targetPosition = new Vector2()
                {
                    X = entity.X,
                    Y = entity.Y
                };

                if (Vector2.Distance(targetPosition, s.TargetPosition) == 0)
                    targetVelocity = 0;

                var position =
                    new Vector2()
                    {
                        X = (float)(entity.X + Math.Cos(angle) * s.Radius) - host.X,
                        Y = (float)(entity.Y + Math.Sin(angle) * s.Radius) - host.Y
                    };
                position.Normalize();
                position *= (hostVelocity + targetVelocity);

                var relativePosition =
                    new Vector2()
                    {
                        X = entity.X - host.X,
                        Y = entity.Y - host.Y
                    };
                relativePosition.Normalize();
                relativePosition *= targetVelocity;

                position += (new Vector2(host.X, host.Y) + relativePosition);

                host.ValidateAndMove(position.X, position.Y);
                host.UpdateCount++;

                s.Angle = (angle * 180) / Math.PI;
                s.Position = position;
                s.TargetPosition = targetPosition;

                state = s;

                Status = CycleStatus.InProgress;

                Log.Info($"[Entity] X: {entity.X:n2} / Y: {entity.Y:n2}");
                Log.Info($"[Host] X: {host.X:n2} / Y: {host.Y:n2}");
                Log.Info($"[State]\n{s.ToString()}\n");
            }

            state = s;
        }
    }
}