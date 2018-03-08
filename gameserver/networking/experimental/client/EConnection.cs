using LoESoft.Core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoESoft.GameServer.networking
{
    public partial class Client
    {
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

                _eserver.TryDisconnect(this);
            }
        }
    }
}
