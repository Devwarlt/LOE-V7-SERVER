#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.outgoing
{
    public class CTT_SEND : OutgoingMessage
    {
        public string CTTSendMessage { get; set; }

        public override MessageID ID => MessageID.CTT_SEND;

        public override Message CreateInstance() => new CTT_SEND();

        protected override void Read(NReader rdr)
        {
            CTTSendMessage = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(CTTSendMessage);
        }
    }
}