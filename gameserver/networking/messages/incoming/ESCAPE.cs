#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class ESCAPE : IncomingMessage
    {
        public override MessageID ID => MessageID.ESCAPE;

        public override Message CreateInstance() => new ESCAPE();

        protected override void Read(NReader rdr) { }

        protected override void Write(NWriter wtr) { }
    }
}