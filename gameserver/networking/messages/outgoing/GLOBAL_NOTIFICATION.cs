#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.outgoing
{
    public class GLOBAL_NOTIFICATION : OutgoingMessage
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public override MessageID ID => MessageID.GLOBAL_NOTIFICATION;

        public override Message CreateInstance() => new GLOBAL_NOTIFICATION();

        protected override void Read(NReader rdr)
        {
            Type = rdr.ReadInt32();
            Text = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Type);
            wtr.WriteUTF(Text);
        }
    }
}