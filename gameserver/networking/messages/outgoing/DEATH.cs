#region

using common;

#endregion

namespace gameserver.networking.outgoing
{
    public class DEATH : OutgoingMessage
    {
        public string AccountId { get; set; }
        public int CharId { get; set; }
        public string Killer { get; set; }
        public int zombieId { get; set; }
        public int zombieType { get; set; }

        public override MessageID ID => MessageID.DEATH;

        public override Message CreateInstance() => new DEATH();

        protected override void Read(NReader rdr)
        {
            AccountId = rdr.ReadUTF();
            CharId = rdr.ReadInt32();
            Killer = rdr.ReadUTF();
            zombieType = rdr.ReadInt32();
            zombieId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(AccountId);
            wtr.Write(CharId);
            wtr.WriteUTF(Killer);
            wtr.Write(zombieType);
            wtr.Write(zombieId);
        }
    }
}