#region

using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity.player;
using LoESoft.GameServer.realm.terrain;

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
                player.ClientTick(time, message);
                CheckLabConditions(player, message);
                player.UpdateCount++;
            }
        }

        private static void CheckLabConditions(Entity player, MOVE packet)
        {
            WmapTile tile = player.Owner.Map[(int)packet.Position.X, (int)packet.Position.Y];
            switch (tile.TileId)
            {
                //Green water
                case 0xa9:
                case 0x82:
                    if (tile.ObjId != 0) return;
                    if (!player.HasConditionEffect(ConditionEffectIndex.Hexed) ||
                        !player.HasConditionEffect(ConditionEffectIndex.Stunned) ||
                        !player.HasConditionEffect(ConditionEffectIndex.Speedy))
                    {
                        player.ApplyConditionEffect(ConditionEffectIndex.Hexed);
                        player.ApplyConditionEffect(ConditionEffectIndex.Stunned);
                        player.ApplyConditionEffect(ConditionEffectIndex.Speedy);
                    }
                    break;
                //Blue water
                case 0xa7:
                case 0x83:
                    if (tile.ObjId != 0) return;
                    if (player.HasConditionEffect(ConditionEffectIndex.Hexed) ||
                        player.HasConditionEffect(ConditionEffectIndex.Stunned) ||
                        player.HasConditionEffect(ConditionEffectIndex.Speedy))
                    {
                        player.ApplyConditionEffect(ConditionEffectIndex.Hexed, 0);
                        player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 0);
                        player.ApplyConditionEffect(ConditionEffectIndex.Speedy, 0);
                    }
                    break;
            }
        }
    }
}