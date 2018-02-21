﻿#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class PLAYERHIT : IncomingMessage
    {
        public byte BulletId { get; set; }
        public int ObjectId { get; set; }

        public override MessageID ID => MessageID.PLAYERHIT;

        public override Message CreateInstance() => new PLAYERHIT();

        protected override void Read(NReader rdr)
        {
            BulletId = rdr.ReadByte();
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
        }
    }
}