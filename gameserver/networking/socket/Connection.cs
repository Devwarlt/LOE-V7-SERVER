using gameserver.networking.error;
using gameserver.networking.outgoing;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using FAILURE = gameserver.networking.outgoing.FAILURE;

namespace gameserver.networking
{
    public partial class Client
    {
        public Task task = Task.Delay(250);

        public string[] time => DateTime.Now.ToString().Split(' ');

        public void _(string accId, RECONNECT msg)
        {
            string response = $"[{time[1]}] [{nameof(Client)}] Reconnect\t->\tplayer id {accId} to {msg.Name}";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(response);
            Console.ResetColor();
        }

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
            RECEIVING_HDR = 18,
            RECEIVING_BODY = 19,
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
            UNKNOW_ERROR_INSTANCE = 255
        }

        public async void Reconnect(RECONNECT msg)
        {
            if (this == null)
                return;

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

                await task;

                Disconnect(DisconnectReason.LOST_CONNECTION);
                return;
            }

            _(Account.AccountId, msg);

            Save();

            await task;

            task.Dispose();

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
                Program.Logger.Error($"[{nameof(Client)}] Save exception:\n{ex}");
            }
        }

        public async void Disconnect(DisconnectReason reason)
        {
            try
            {
                Save();

                await task;

                task.Dispose();

                if (Socket == null || Account == null || State == ProtocolState.Disconnected)
                    return;

                Log.Write($"[({(int)reason}) {reason.ToString()}] Disconnect player '{Account.Name} (Account ID: {Account.AccountId})' from IP '{Socket.RemoteEndPoint.ToString().Split(':')[0]}'.");

                State = ProtocolState.Disconnected;

                Manager.Disconnect(this);

                Socket?.Close();
            }
            catch (Exception e)
            {
                Program.Logger.Error($"[{nameof(Client)}] Disconnect exception:\n{e}");
            }
        }
    }
}