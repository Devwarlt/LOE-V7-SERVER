﻿#region

using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.networking.incoming
{
    public class GUILDINVITE : IncomingMessage
    {
        public string Name { get; set; }

        public override MessageID ID => MessageID.GUILDINVITE;

        public override Message CreateInstance() => new GUILDINVITE();

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}