﻿#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.outgoing
{
    public class VERIFY_EMAIL : OutgoingMessage
    {
        public override MessageID ID => MessageID.VERIFY_EMAIL;

        public override Message CreateInstance() => new VERIFY_EMAIL();

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}