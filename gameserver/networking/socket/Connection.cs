using LoESoft.Core.models;
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
            RECEIVING_MESSAGE = 18,
            RECEIVING_DATA = 19,
            ERROR_WHEN_HANDLING_MESSAGE = 20,
            SOCKET_ERROR_DETECTED = 21,
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
            CONNECTION_RESET = 36,
            SOCKET_ERROR = 37,
            CONNECTION_LOST = 38,
            OVERFLOW_EXCEPTION = 39,
            NETWORK_TICKER_DISCONNECT = 40,
            OLD_CLIENT_DISCONNECT = 41,
            BYTES_NOT_READY = 42,
            UNKNOW_ERROR_INSTANCE = 255
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

                Manager.TryDisconnect(this, DisconnectReason.LOST_CONNECTION);
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
                    Manager.Database.SaveCharacter(Account, Character, false);
                if (Account != null)
                    Manager.Database.ReleaseLock(Account);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}