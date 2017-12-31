#region

using System;
using System.Xml.Linq;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.realm.entity.merchant
{
    partial class Merchant
    {
        public void Recreate(Merchant x)
        {
            try
            {
                var mrc = new Merchant(Manager, x.ObjectType, x.Owner);
                mrc.Move(x.X, x.Y);
                var w = Owner;
                Owner.LeaveWorld(this);
                w.Timers.Add(new WorldTimer(Random.Next(30, 60) * 1000, (world, time) => w.EnterWorld(mrc)));
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public override void Buy(Player player)
        {
            if (ObjectType == 0x01ca) //Merchant
            {
                Price *= player.AccountPerks.MerchantDiscount();
                if (TryDeduct(player))
                {
                    for (var i = 4; i < player.Inventory.Length; i++)
                    {
                        try
                        {
                            XElement ist;
                            Manager.GameData.ObjectTypeToElement.TryGetValue((ushort)MType, out ist);
                            if (player.Inventory[i] == null &&
                                (player.SlotTypes[i] == 10 ||
                                 player.SlotTypes[i] == Convert.ToInt16(ist.Element("SlotType").Value)))
                            // Exploit fix - No more mnovas as weapons!
                            {
                                player.Inventory[i] = Manager.GameData.Items[(ushort)MType];

                                switch(Currency)
                                {
                                    case CurrencyType.Fame:
                                        {
                                            Manager.Database.UpdateFame(player.Client.Account, -Price);
                                            player.CurrentFame = player.Client.Account.Fame;
                                        }
                                        break;
                                    case CurrencyType.Gold:
                                        {
                                            Manager.Database.UpdateCredit(player.Client.Account, -Price);
                                            player.Credits = player.Client.Account.Credits;
                                        }
                                        break;
                                    case CurrencyType.FortuneTokens:
                                        {
                                            Manager.Database.UpdateTokens(player.Client.Account, -Price);
                                            player.Tokens = player.Client.Account.FortuneTokens;
                                        }
                                        break;
                                    default: break;
                                }
                                player.Client.SendMessage(new BUYRESULT
                                {
                                    Result = 0,
                                    Message = "{\"key\":\"server.buy_success\"}"
                                });
                                MRemaining--;
                                player.UpdateCount++;
                                player.SaveToCharacter();
                                UpdateCount++;
                                return;
                            }
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    player.Client.SendMessage(new BUYRESULT
                    {
                        Result = 0,
                        Message = "{\"key\":\"server.inventory_full\"}"
                    });
                }
                else
                {
                    if (player.Stars < RankReq)
                    {
                        player.Client.SendMessage(new BUYRESULT
                        {
                            Result = 0,
                            Message = "{\"key\":\"server.not_enough_star\"}"
                        });
                        return;
                    }
                    switch (Currency)
                    {
                        case CurrencyType.Gold:
                            player.Client.SendMessage(new BUYRESULT
                            {
                                Result = BUY_NO_GOLD,
                                Message = "{\"key\":\"server.not_enough_gold\"}"
                            });
                            break;

                        case CurrencyType.Fame:
                            player.Client.SendMessage(new BUYRESULT
                            {
                                Result = BUY_NO_FAME,
                                Message = "{\"key\":\"server.not_enough_fame\"}"
                            });
                            break;

                        case CurrencyType.FortuneTokens:
                            player.Client.SendMessage(new BUYRESULT
                            {
                                Result = BUY_NO_FORTUNETOKENS,
                                Message = "{\"key\":\"server.not_enough_fortunetokens\"}"
                            });
                            break;
                    }
                }
            };
        }

        protected override bool TryDeduct(Player player)
        {
            var acc = player.Client.Account;
            if (player.Stars < RankReq) return false;

            if (Currency == CurrencyType.Fame)
                if (acc.Fame < Price) return false;

            if (Currency == CurrencyType.Gold)
                if (acc.Credits < Price) return false;

            if (Currency == CurrencyType.FortuneTokens)
                if (acc.FortuneTokens < Price) return false;
            return true;
        }
    }
}
