#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class CTT_RECEIVE : IncomingMessage
    {
        public string CTTAuth { get; set; }

        public override MessageID ID => MessageID.CTT_RECEIVE;

        public override Message CreateInstance() => new CTT_RECEIVE();

        protected override void Read(NReader rdr)
        {
            CTTAuth = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(CTTAuth);
        }
    }
}