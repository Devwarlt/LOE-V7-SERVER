#region

using System;
using System.Collections.Generic;
using System.Linq;
using LoESoft.GameServer.networking;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.Core;
using LoESoft.Core.config;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        public bool Activate(RealmTime time, Item item, USEITEM pkt)
        {
            bool endMethod = false;

            Position target = pkt.ItemUsePos;
            MP -= item.MpCost;

            IContainer con = Owner?.GetEntity(pkt.SlotObject.ObjectId) as IContainer;
            if (con == null)
                return true;

            Random rnd = new Random();

            foreach (ActivateEffect eff in item.ActivateEffects)
            {
                switch (eff.Effect)
                {
                    case ActivateEffects.RemoveNegativeConditions:
                        {
                            this?.Aoe(eff.Range / 2, true, player => ApplyConditionEffect(NegativeEffs));
                            BroadcastSync(new SHOWEFFECT
                            {
                                EffectType = EffectType.Nova,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position { X = eff.Range / 2 }
                            }, p => this?.Dist(p) < 25);
                        }
                        break;
                    case ActivateEffects.RemoveNegativeConditionsSelf:
                        {
                            ApplyConditionEffect(NegativeEffs);
                            Owner?.BroadcastMessage(new SHOWEFFECT
                            {
                                EffectType = EffectType.Nova,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position { X = 1 }
                            }, null);
                        }
                        break;
                    case ActivateEffects.ConditionEffectSelf:
                        {
                            int durationCES = eff.DurationMS;

                            if (eff.UseWisMod)
                                durationCES = (int)(eff.DurationSec * 1000);

                            uint color = 0xffffffff;

                            switch (eff.ConditionEffect.Value)
                            {
                                case ConditionEffectIndex.Damaging:
                                    color = 0xffff0000;
                                    break;
                                case ConditionEffectIndex.Berserk:
                                    color = 0x808080;
                                    break;
                            }

                            ApplyConditionEffect(new ConditionEffect
                            {
                                Effect = eff.ConditionEffect.Value,
                                DurationMS = durationCES
                            });

                            Owner?.BroadcastMessage(new SHOWEFFECT
                            {
                                EffectType = EffectType.Nova,
                                TargetId = Id,
                                Color = new ARGB(color),
                                PosA = new Position { X = 2F }
                            }, null);
                        }
                        break;
                    case ActivateEffects.AccountLifetime:
                        {
                            if (Database.Names.Contains(Name))
                            {
                                SendInfo("Players without valid name couldn't use this feature. Please name your character to continue.");
                                return true;
                            }

                            if (AccountType == (int)Core.config.AccountType.VIP_ACCOUNT)
                            {
                                SendInfo($"You can only use {item.DisplayId} when your VIP account lifetime over.");
                                return true;
                            }

                            if (AccountType >= (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT)
                            {
                                SendInfo($"Only VIP account type can use {item.DisplayId}.");
                                return true;
                            }

                            List<Message> _outgoing = new List<Message>();
                            World _world = GameServer.Manager.GetWorld(Owner.Id);
                            DbAccount acc = Client.Account;
                            int days = eff.Amount;

                            SendInfo($"Success! You received {eff.Amount} day{(eff.Amount > 1 ? "s" : "")} as account lifetime to your VIP account type when {item.DisplayId} was consumed!");

                            NOTIFICATION _notification = new NOTIFICATION
                            {
                                Color = new ARGB(0xFFFFFF),
                                ObjectId = Id,
                                Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Success!\"}}"
                            };

                            _outgoing.Add(_notification);

                            SHOWEFFECT _showeffect = new SHOWEFFECT
                            {
                                Color = new ARGB(0xffddff00),
                                EffectType = EffectType.Nova,
                                PosA = new Position { X = 2 }
                            };

                            _outgoing.Add(_showeffect);

                            Owner.BroadcastMessage(_outgoing, null);

                            acc.AccountLifetime = DateTime.Now;
                            acc.AccountLifetime = acc.AccountLifetime.AddDays(days);
                            acc.AccountType = (int)Core.config.AccountType.VIP_ACCOUNT;
                            acc.Flush();
                            acc.Reload();

                            UpdateCount++;

                            SendInfo("Reconnecting...");

                            RECONNECT _reconnect = new RECONNECT
                            {
                                GameId = (int)TownID.ISLE_OF_APPRENTICES, // change to Drasta Citadel in future versions!
                                Host = string.Empty,
                                Key = Empty<byte>.Array,
                                Name = "Nexus",
                                Port = Settings.GAMESERVER.PORT
                            };

                            _world.Timers.Add(new WorldTimer(2000, (w, t) => Client.Reconnect(_reconnect)));
                        }
                        break;
                    default: return true;
                }
            }
            UpdateCount++;
            return endMethod;
        }
    }
}