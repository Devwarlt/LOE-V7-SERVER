#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class MOVE : IncomingMessage
    {
        public Position Position { get; set; }

        public override MessageID ID => MessageID.MOVE;

        public override Message CreateInstance() => new MOVE();

        protected override void Read(NReader rdr)
        {
            Position = Position.Read(rdr);
        }

        protected override void Write(NWriter wtr)
        {
            Position.Write(wtr);
        }
    }
}