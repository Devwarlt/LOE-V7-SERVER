﻿#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class TELEPORT : IncomingMessage
    {
        public int ObjectId { get; set; }

        public override MessageID ID => MessageID.TELEPORT;

        public override Message CreateInstance() => new TELEPORT();

        protected override void Read(NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}