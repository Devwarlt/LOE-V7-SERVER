﻿#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class UPDATEACK : IncomingMessage
    {
        public override MessageID ID => MessageID.UPDATEACK;

        public override Message CreateInstance() => new UPDATEACK();

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}