#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class CHECKCREDITS : IncomingMessage
    {
        public override MessageID ID => MessageID.CHECKCREDITS;

        public override Message CreateInstance() => new CHECKCREDITS();

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}