#region

using System.Linq;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;
using System.Collections.Generic;
using System;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class UseItemHandler : MessageHandlers<USEITEM>
    {
        public override MessageID ID => MessageID.USEITEM;

        protected override void HandleMessage(Client client, USEITEM message)
        {
            if (client.Player.Owner == null)
                return;
            try
            {
                Manager.Logic.AddPendingAction(t =>
                {
                    IContainer container = client.Player.Owner.GetEntity(message.SlotObject.ObjectId) as IContainer;

                    if (container == null)
                        return;

                    Item item = container.Inventory[message.SlotObject.SlotId];

                    if (item != null)
                    {
                        if (!client.Player.Activate(t, item, message))
                        {
                            if (item.Consumable)
                            {
                                if (item.SuccessorId != null)
                                {
                                    if (item.SuccessorId != item.ObjectId)
                                    {
                                        container.Inventory[message.SlotObject.SlotId] = GameServer.Manager.GameData.Items[GameServer.Manager.GameData.IdToObjectType[item.SuccessorId]];
                                        client.Player.Owner.GetEntity(message.SlotObject.ObjectId).UpdateCount++;
                                    }
                                }
                                else
                                {
                                    container.Inventory[message.SlotObject.SlotId] = null;
                                    client.Player.Owner.GetEntity(message.SlotObject.ObjectId).UpdateCount++;
                                }

                                if (container is OneWayContainer)
                                {
                                    List<int> giftsList = client.Account.Gifts.ToList();
                                    giftsList.Remove(message.SlotObject.ObjectType);
                                    int[] result = giftsList.ToArray();
                                    client.Account.Gifts = result;
                                    client.Account.Flush();
                                }
                            }
                        }
                    }

                    ((Entity)container).UpdateCount++;

                    client.Player.UpdateCount++;
                    client.Player.SaveToCharacter();
                }, PendingPriority.Networking);
            }
            catch (NullReferenceException ex)
            {
                GameServer.log.Error(ex);
                return;
            }
        }
    }
}