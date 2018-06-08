#region

using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class MoveHandler : MessageHandlers<MOVE>
    {
        public override MessageID ID => MessageID.MOVE;

        protected override void HandleMessage(Client client, MOVE message) => Manager.Logic.AddPendingAction(t => Handle(client.Player, t, message), PendingPriority.Networking);

        private void Handle(Player player, RealmTime time, MOVE message)
        {
            if (player?.Owner == null || player.HasConditionEffect(ConditionEffectIndex.Paralyzed) || message.Position.X == -1 || message.Position.Y == -1)
                return;

            float newX = message.Position.X;
            float newY = message.Position.Y;

            if (newX != player.X || newY != player.Y)
            {
                player.CharPosition = new Position(x: newX, y: newY, town: player.Owner.Id);
                player.Client.Character.CharPosition = player.ProcessPosition(player.CharPosition);
                player.Move(newX, newY);
                player.UpdateCount++;
            }
        }
    }
}