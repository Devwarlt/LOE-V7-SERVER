#region

using System;

#endregion

namespace LoESoft.GameServer.logic
{
    public struct Cooldown
    {
        public readonly int CoolDown;
        public readonly int Variance;

        public Cooldown(int cooldown = 0, int variance = 0)
        {
            CoolDown = cooldown;
            Variance = variance;
        }

        public Cooldown Normalize() => CoolDown == 0 ? 1000 : CoolDown;

        public Cooldown Normalize(int def) => CoolDown == 0 ? def : this;

        public int Next(Random rand) => Variance == 0 ? CoolDown : CoolDown + rand.Next(-Variance, Variance + 1);

        public static implicit operator Cooldown(int cooldown) => new Cooldown(cooldown, 0);
    }
}