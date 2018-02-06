#region

using System.Linq;
using common;
using System.Text;
using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm;
using gameserver.realm.world;
using FAILURE = gameserver.networking.outgoing.FAILURE;
using System;
using gameserver.networking.error;
using static gameserver.networking.Client;
using common.config;

#endregion

namespace gameserver.networking.handlers
{
    internal class HelloHandler : MessageHandlers<HELLO>
    {
        private void _(string msg) => Log.Write(nameof(HelloHandler), msg);

        public override MessageID ID => MessageID.HELLO;

        protected override void HandleMessage(Client client, HELLO message)
        {
            Tuple<string, bool> checkGameVersion = CheckGameVersion(message.BuildVersion);
            if (checkGameVersion.Item2)
            {
                client.SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.JSON_DIALOG,
                    ErrorDescription =
                        JSONErrorIDHandler.
                            FormatedJSONError(
                                errorID: ErrorIDs.OUTDATED_CLIENT,
                                labels: new[] { "{CLIENT_BUILD_VERSION}", "{SERVER_BUILD_VERSION}" },
                                arguments: new[] { message.BuildVersion, checkGameVersion.Item1 }
                            )
                });
                Manager.TryDisconnect(client, DisconnectReason.OUTDATED_CLIENT);
                return;
            }
            DbAccount acc;
            LoginStatus s1 = Manager.Database.Verify(message.GUID, message.Password, out acc);
            if (s1 == LoginStatus.AccountNotExists)
            {
                RegisterStatus s2 = Manager.Database.Register(message.GUID, message.Password, true, out acc); //Register guest but do not allow join game.
                client.SendMessage(new FAILURE()
                {
                    ErrorId = (int)FailureIDs.JSON_DIALOG,
                    ErrorDescription =
                        JSONErrorIDHandler.
                            FormatedJSONError(
                                errorID: ErrorIDs.DISABLE_GUEST_ACCOUNT,
                                labels: null,
                                arguments: null
                            )
                });
                Manager.TryDisconnect(client, DisconnectReason.DISABLE_GUEST_ACCOUNT);
                return;
            }
            else if (s1 == LoginStatus.InvalidCredentials)
            {
                client.SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.DEFAULT,
                    ErrorDescription = "Bad login."
                });
                Manager.TryDisconnect(client, DisconnectReason.BAD_LOGIN);
            }
            client.ConnectedBuild = message.BuildVersion;
            client.Account = acc;
            ConnectionProtocol TryConnect = Manager.TryConnect(client);
            if (!TryConnect.connected)
            {
                client.Account = null;
                ErrorIDs errorID = TryConnect.errorID;
                string[] labels;
                string[] arguments;
                DisconnectReason reason;
                switch (errorID)
                {
                    case ErrorIDs.SERVER_FULL:
                        {
                            labels = new[] { "{MAX_USAGE}" };
                            arguments = new[] { Manager.MaxClients.ToString() };
                            reason = DisconnectReason.SERVER_FULL;
                        }
                        break;
                    case ErrorIDs.ACCOUNT_BANNED:
                        {
                            labels = new[] { "{CLIENT_NAME}" };
                            arguments = new[] { acc.Name };
                            reason = DisconnectReason.ACCOUNT_BANNED;
                        }
                        break;
                    case ErrorIDs.INVALID_DISCONNECT_KEY:
                        {
                            labels = new[] { "{CLIENT_NAME}" };
                            arguments = new[] { acc.Name };
                            reason = DisconnectReason.INVALID_DISCONNECT_KEY;
                        }
                        break;
                    case ErrorIDs.LOST_CONNECTION:
                        {
                            labels = new[] { "{CLIENT_NAME}" };
                            arguments = new[] { acc.Name };
                            reason = DisconnectReason.LOST_CONNECTION;
                        }
                        break;
                    default:
                        {
                            labels = new[] { "{UNKNOW_ERROR_INSTANCE}" };
                            arguments = new[] { "connection aborted by unexpected protocol at line <b>340</b> or line <b>346</b> from 'TryConnect' function in RealmManager for security reasons" };
                            reason = DisconnectReason.UNKNOW_ERROR_INSTANCE;
                        }
                        break;
                }
                client.SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.JSON_DIALOG,
                    ErrorDescription =
                        JSONErrorIDHandler.
                            FormatedJSONError(
                                errorID: errorID,
                                labels: labels,
                                arguments: arguments
                            )
                });
                Manager.TryDisconnect(client, reason);
                return;
            }
            else
            {
                if (message.GameId == World.NEXUS_LIMBO)
                    message.GameId = World.NEXUS_ID;

                World world = Manager.GetWorld(message.GameId);

                if (world == null && message.GameId == World.TUT_ID)
                    world = Manager.AddWorld(new Tutorial(false));

                if (!Manager.Database.AcquireLock(acc))
                {
                    int accountInUseTime = Manager.Database.GetLockTime(acc);
                    client.SendMessage(new FAILURE
                    {
                        ErrorId = (int)FailureIDs.JSON_DIALOG,
                        ErrorDescription =
                            JSONErrorIDHandler
                                .FormatedJSONError(
                                    errorID: ErrorIDs.ACCOUNT_IN_USE,
                                    labels: new[] { "{CLIENT_NAME}", "{TIME_LEFT}" },
                                    arguments: new[] { acc.Name, $"{accountInUseTime} second{(accountInUseTime > 1 ? "s" : "")}" }
                                )
                    });
                    Manager.TryDisconnect(client, DisconnectReason.ACCOUNT_IN_USE);
                    return;
                }

                if (acc.AccountType == (int)accountType.VIP_ACCOUNT)
                {
                    DateTime _currentTime = DateTime.Now;
                    DateTime _vipRegistration = acc.AccountLifetime;
                    if (_vipRegistration <= _currentTime)
                    {
                        acc.AccountType = (int)accountType.FREE_ACCOUNT;
                        acc.Flush();
                        acc.Reload();
                        FAILURE _failure = new FAILURE();
                        _failure.ErrorId = (int)FailureIDs.JSON_DIALOG;
                        _failure.ErrorDescription =
                            JSONErrorIDHandler
                                .FormatedJSONError(
                                    errorID: ErrorIDs.VIP_ACCOUNT_OVER,
                                    labels: new[] { "{CLIENT_NAME}", "{SERVER_TIME}", "{REGISTRATION_TIME}", "{CURRENT_TIME}" },
                                    arguments: new[] { acc.Name, string.Format(new DateProvider(), "{0}", DateTime.Now), string.Format(new DateProvider(), "{0}", acc.AccountLifetime.AddDays(-30)), string.Format(new DateProvider(), "{0}", acc.AccountLifetime) }
                                );
                        client.SendMessage(_failure);
                        Manager.TryDisconnect(client, DisconnectReason.VIP_ACCOUNT_OVER);
                        return;
                    }
                }

                if (world == null)
                {
                    client.SendMessage(new FAILURE
                    {
                        ErrorId = (int)FailureIDs.DEFAULT,
                        ErrorDescription = "Invalid world."
                    });
                    Manager.TryDisconnect(client, DisconnectReason.INVALID_WORLD);
                    return;
                }

                if (world.NeedsPortalKey)
                {
                    if (!world.PortalKey.SequenceEqual(message.Key))
                    {
                        client.SendMessage(new FAILURE
                        {
                            ErrorId = (int)FailureIDs.DEFAULT,
                            ErrorDescription = "Invalid portal key."
                        });
                        Manager.TryDisconnect(client, DisconnectReason.INVALID_PORTAL_KEY);
                        return;
                    }

                    if (world.PortalKeyExpired)
                    {
                        client.SendMessage(new FAILURE
                        {
                            ErrorId = (int)FailureIDs.DEFAULT,
                            ErrorDescription = "Portal key expired."
                        });
                        Manager.TryDisconnect(client, DisconnectReason.PORTAL_KEY_EXPIRED);
                        return;
                    }
                }
                if (message.MapInfo.Length > 0 || world.Id == -6) //Test World
                    (world as Test).LoadJson(Encoding.Default.GetString(message.MapInfo));

                if (world.IsLimbo)
                    world = world.GetInstance(client);
                
                client.Random = new wRandom(world.Seed);
                client.TargetWorld = world.Id;
                client.SendMessage(new MAPINFO
                {
                    Width = world.Map.Width,
                    Height = world.Map.Height,
                    Name = world.Name,
                    Seed = world.Seed,
                    ClientWorldName = world.ClientWorldName,
                    Difficulty = world.Difficulty,
                    Background = world.Background,
                    AllowTeleport = world.AllowTeleport,
                    ShowDisplays = world.ShowDisplays,
                    ClientXML = world.ClientXml,
                    ExtraXML = Manager.GameData.AdditionXml,
                    Music = world.Name
                });
                client.State = ProtocolState.Handshaked;
            }
        }
    }
}