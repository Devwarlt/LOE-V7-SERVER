#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.outgoing
{
    public class TRADEREQUESTED : OutgoingMessage
    {
        public string Name { get; set; }

        public override MessageID ID => MessageID.TRADEREQUESTED;

        public override Message CreateInstance() => new TRADEREQUESTED();

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}