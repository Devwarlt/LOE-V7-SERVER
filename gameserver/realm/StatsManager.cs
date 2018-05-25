#region

using System;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.realm
{
    public class StatsManager
    {
        private readonly Player player;

        public StatsManager(Player player, uint seed)
        {
            this.player = player;
            Random = new DamageRandom(seed);
        }

        public DamageRandom Random { get; }

        public float GetAttackDamage(int min, int max)
        {
            return Random.Obf6((uint)min, (uint)max) * DamageModifier();
        }

        public readonly static float MinAttackFrequency = 0.0015f;
        public readonly static float MaxAttackFrequency = 0.008f;

        public float GetAttackFrequency()
        {
            // TODO.
            return 0;
        }

        private float DamageModifier()
        {
            // TODO.
            return 0;
        }

        public static float GetDefenseDamage(Entity host, int dmg, int def)
        {
            // TODO.
            return 0;
        }

        public float GetDefenseDamage(int dmg, bool noDef)
        {
            // TODO.
            return 0;
        }

        public static float GetSpeed(Entity entity, float stat)
        {
            // TODO.
            return 0;
        }

        public float GetSpeed()
        {
            // TODO.
            return 0;
        }

        public float GetHPRegen()
        {
            // TODO.
            return 0;
        }

        public float GetMPRegen()
        {
            // TODO.
            return 0;
        }

        public float GetDex()
        {
            // TODO.
            return 0;
        }

        public class DamageRandom
        {
            public DamageRandom(uint seed = 1)
            {
                Seed = seed;
            }

            public uint Seed { get; private set; }

            public static uint Obf1()
            {
                return (uint)Math.Round(new Random().NextDouble() * (uint.MaxValue - 1) + 1);
            }

            public uint Obf2()
            {
                return Obf3();
            }

            public float Obf4()
            {
                return Obf3() / 2147483647;
            }

            public float Obf5(float param1 = 0.0f, float param2 = 1.0f)
            {
                float _loc3_ = Obf3() / 2147483647;
                float _loc4_ = Obf3() / 2147483647;
                float _loc5_ = (float)Math.Sqrt(-2 * (float)Math.Log(_loc3_)) * (float)Math.Cos(2 * _loc4_ * Math.PI);
                return param1 + _loc5_ * param2;
            }

            public uint Obf6(uint param1, uint param2)
            {
                if (param1 == param2)
                {
                    return param1;
                }
                return param1 + Obf3() % (param2 - param1);
            }

            public float Obf7(float param1, float param2)
            {
                return param1 + (param2 - param1) * Obf4();
            }

            private uint Obf3()
            {
                uint _loc1_ = 0;
                uint _loc2_ = 0;
                _loc2_ = 16807 * (Seed & 65535);
                _loc1_ = 16807 * (Seed >> 16);
                _loc2_ = _loc2_ + ((_loc1_ & 32767) << 16);
                _loc2_ = _loc2_ + (_loc1_ >> 15);
                if (_loc2_ > 2147483647)
                {
                    _loc2_ = _loc2_ - 2147483647;
                }
                return Seed = _loc2_;
            }
        }
    }
}