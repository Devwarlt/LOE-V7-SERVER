#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class PLAYERSHOOT : IncomingMessage
    {
        public byte BulletId { get; set; }
        public short ContainerType { get; set; }
        public Position Position { get; set; }
        public float Angle { get; set; }
        public float AttackPeriod { get; set; }
        public int AttackAmount { get; set; }

        public override MessageID ID => MessageID.PLAYERSHOOT;

        public override Message CreateInstance() => new PLAYERSHOOT();

        protected override void Read(NReader rdr)
        {
            BulletId = rdr.ReadByte();
            ContainerType = rdr.ReadInt16();
            Position = Position.Read(rdr);
            Angle = rdr.ReadSingle();
            AttackPeriod = rdr.ReadSingle();
            AttackAmount = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(ContainerType);
            Position.Write(wtr);
            wtr.Write(Angle);
            wtr.Write(AttackPeriod);
            wtr.Write(AttackAmount);
        }
    }
}