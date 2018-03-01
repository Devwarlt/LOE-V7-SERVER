﻿using LoESoft.Core.models;
using LoESoft.GameServer.networking.error;
using LoESoft.GameServer.networking.outgoing;
using System;
using FAILURE = LoESoft.GameServer.networking.outgoing.FAILURE;

namespace LoESoft.GameServer.networking
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public partial class Client
    {
        public enum DisconnectReason : byte
        {
            FAILED_TO_LOAD_CHARACTER = 1,
            OUTDATED_CLIENT = 2,
            DISABLE_GUEST_ACCOUNT = 3,
            BAD_LOGIN = 4,
            SERVER_FULL = 5,
            ACCOUNT_BANNED = 6,
            INVALID_DISCONNECT_KEY = 7,
            LOST_CONNECTION = 8,
            ACCOUNT_IN_USE = 9,
            INVALID_WORLD = 10,
            INVALID_PORTAL_KEY = 11,
            PORTAL_KEY_EXPIRED = 12,
            CHARACTER_IS_DEAD = 13,
            HP_POTION_CHEAT_ENGINE = 14,
            MP_POTION_CHEAT_ENGINE = 15,
            STOPING_SERVER = 16,
            SOCKET_IS_NOT_CONNECTED = 17,
            INVALID_INCOMING_BYTES = 18,
            INVALID_TRANSFERRED_BYTES = 19,
            ERROR_WHEN_HANDLING_MESSAGE = 20,
            INVALID_SOCKET_PROCESSING = 21,
            PROCESS_POLICY_FILE = 22,
            RESTART = 23,
            PLAYER_KICK = 24,
            PLAYER_BANNED = 25,
            CHARACTER_IS_DEAD_ERROR = 26,
            CHEAT_ENGINE_DETECTED = 27,
            RECONNECT_TO_CASTLE = 28,
            REALM_MANAGER_DISCONNECT = 29,
            STOPPING_REALM_MANAGER = 30,
            DUPER_DISCONNECT = 31,
            ACCESS_DENIED = 32,
            VIP_ACCOUNT_OVER = 33,
            DEXTERITY_HACK_MOD = 34,
            RECONNECT = 35,
            UNKNOWN_MESSAGE = 36,
            CONNECTION_RESETED = 37,
            SOCKET_ERROR = 38,
            BYTES_NOT_READY = 39,
            UNKNOW_ERROR_INSTANCE = 255
        }

        public void TryDisconnect(DisconnectReason reason)
        {
            using (TimedLock.Lock(DcLock))
            {
                if (State == ProtocolState.Disconnected)
                    return;

                State = ProtocolState.Disconnected;

                Log.Info($"[({(int)reason}) {reason.ToString()}] Disconnect player '{Account.Name} (Account ID: {Account.AccountId})'.");

                if (Account != null)
                    try
                    {
                        Save();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{e.Message}\n{e.StackTrace}");
                    }
                
                _server.TryDisconnect(this);
            }
        }

        public void Reconnect(RECONNECT msg)
        {
            if (Account == null)
            {
                string[] labels = new string[] { "{CLIENT_NAME}" };
                string[] arguments = new string[] { Account.Name };

                SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.JSON_DIALOG,
                    ErrorDescription =
                        JSONErrorIDHandler.
                            FormatedJSONError(
                                errorID: ErrorIDs.LOST_CONNECTION,
                                labels: labels,
                                arguments: arguments
                            )
                });

                _manager.TryDisconnect(this, DisconnectReason.LOST_CONNECTION);
                return;
            }

            Log.Info($"[({(int)DisconnectReason.RECONNECT}) {DisconnectReason.RECONNECT.ToString()}] Reconnect player '{Account.Name} (Account ID: {Account.AccountId})' to {msg.Name}.");

            Save();

            SendMessage(msg);
        }

        public void Save()
        {
            try
            {
                Player?.SaveToCharacter();

                if (Character != null)
                    _manager.Database.SaveCharacter(Account, Character, false);
                if (Account != null)
                    _manager.Database.ReleaseLock(Account);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}