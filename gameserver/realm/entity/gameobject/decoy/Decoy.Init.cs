#region

using Mono.Game;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.realm.entity
{
    partial class Decoy : GameObject, IPlayer
    {
        public Decoy(Player player, int duration, float speed)
            : base(0x0715, duration, true, true, true)
        {
            this.player = player;
            this.duration = duration;
            this.speed = speed;
            IsJoinedWorld = false;

            Position? history = player.TryGetHistory(1000);

            if (history == null)
                direction = GetRandDirection();
            else
            {
                direction = new Vector2(player.X - history.Value.X, player.Y - history.Value.Y);

                if (direction.LengthSquared() == 0)
                    direction = GetRandDirection();
                else
                    direction.Normalize();
            }
        }

        private long JoinedWorld { get; set; }
        private bool IsJoinedWorld { get; set; }

        public override void Tick(RealmTime time)
        {
            if (!IsJoinedWorld)
            {
                IsJoinedWorld = true;
                JoinedWorld = time.TotalElapsedMs;
            }
            else
            {
                if (duration > time.TotalElapsedMs - JoinedWorld)
                    ValidateAndMove(X + direction.X * EntitySpeed(speed, time), Y + direction.Y * EntitySpeed(speed, time));
                else
                    Owner?.BroadcastPacket(new SHOWEFFECT()
                    {
                        EffectType = EffectType.Nova,
                        Color = new ARGB(0xffff0000),
                        TargetId = Id,
                        PosA = new Position() { X = 1 }
                    }, null);
            }

            base.Tick(time);
        }
    }
}