#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class URL : IncomingMessage
    {
        public string Domain { get; set; }

        public override MessageID ID => MessageID.URL;

        public override Message CreateInstance() => new URL();

        protected override void Read(NReader rdr) => Domain = rdr.ReadUTF();

        protected override void Write(NWriter wtr) => wtr.WriteUTF(Domain);
    }
}