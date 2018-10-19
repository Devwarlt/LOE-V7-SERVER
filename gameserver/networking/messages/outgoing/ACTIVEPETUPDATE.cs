#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.outgoing
{
    public class ACTIVEPETUPDATE : OutgoingMessage
    {
        public int PetId { get; set; }

        public override MessageID ID => MessageID.ACTIVEPETUPDATE;

        public override Message CreateInstance() => new ACTIVEPETUPDATE();

        protected override void Read(NReader rdr)
        {
            PetId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(PetId);
        }
    }
}