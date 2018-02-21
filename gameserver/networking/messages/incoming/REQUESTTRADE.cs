#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class REQUESTTRADE : IncomingMessage
    {
        public string Name { get; set; }

        public override MessageID ID => MessageID.REQUESTTRADE;

        public override Message CreateInstance() => new REQUESTTRADE();

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