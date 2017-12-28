#region

using common;

#endregion

namespace gameserver.networking.outgoing
{
    public class IMMINENT_ARENA_WAVE : OutgoingMessage
    {
        public int CurrentRuntime { get; set; }
        public int Wave { get; set; }

        public override MessageID ID => MessageID.IMMINENT_ARENA_WAVE;

        public override Message CreateInstance() => new IMMINENT_ARENA_WAVE();

        protected override void Read(NReader rdr)
        {
            CurrentRuntime = rdr.ReadInt32();
            Wave = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(CurrentRuntime);
            wtr.Write(Wave);
        }
    }
}