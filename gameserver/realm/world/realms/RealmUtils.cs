﻿using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.player;
using LoESoft.GameServer.realm.terrain;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LoESoft.GameServer.realm
{
    internal partial class Realm
    {
        public static double GetNormal(Random rand) =>
            Math.Sqrt(-2.0 * Math.Log(GetUniform(rand))) * Math.Sin(2.0 * Math.PI * GetUniform(rand));

        public static double GetNormal(Random rand, double mean, double standardDeviation) =>
            mean + standardDeviation * GetNormal(rand);

        public static double GetUniform(Random rand) =>
            ((uint)(rand.NextDouble() * uint.MaxValue) + 1.0) * 2.328306435454494e-10;

        public int CountEnemies(params string[] enemies)
        {
            List<ushort> enemyList = new List<ushort>();

            foreach (var i in enemies)
            {
                try
                {
                    enemyList.Add(Program.Manager.GameData.IdToObjectType[i]);
                }
                catch (Exception) { }
            }
            return world.Enemies.Count(i => enemyList.Contains(i.Value.ObjectType));
        }

        private ushort GetRandomObjType(Tuple<string, double>[] dat)
        {
            var p = rand.NextDouble();
            double n = 0;
            ushort objType = 0;
            foreach (var k in dat)
            {
                n += k.Item2;
                if (n > p)
                {
                    objType = Program.Manager.GameData.IdToObjectType[k.Item1];
                    break;
                }
            }
            return objType;
        }

        private void RecalculateEnemyCount()
        {
            for (var i = 0; i < enemyCounts.Length; i++)
                enemyCounts[i] = 0;
            foreach (var i in world.Enemies)
            {
                if (i.Value.Terrain == WmapTerrain.None) continue;
                enemyCounts[(int)i.Value.Terrain - 1]++;
            }
        }

        public void Tick(RealmTime time)
        {
            if (!disposed)
            {
                if (HandleHeroes())
                {
                    if (!ClosingStarted)
                    {
                        InitCloseRealm();
                    }
                }
                else
                {
                    if (time.TotalElapsedMs - prevTick > 25000)
                    {
                        if (x % 2 == 0)
                            HandleAnnouncements();
                        if (x % 6 == 0)
                            EnsurePopulation();
                        x++;
                        prevTick = time.TotalElapsedMs;
                    }
                }
            }
        }

        private void BroadcastMsg(string message) =>
            Program.Manager.Chat.Oryx(world, message);

        private void SendMsg(Player player, string message, string src = "") =>
            player.Client.SendMessage(new TEXT
            {
                Name = src,
                ObjectId = -1,
                Stars = -1,
                BubbleTime = 0,
                Recipient = "",
                Text = message,
                CleanText = "",
                NameColor = 0x123456,
                TextColor = 0x123456
            });

        private void HandleAnnouncements()
        {
            Tuple<string, TauntData> taunt = criticalEnemies[rand.Next(0, criticalEnemies.Length)];
            int count = 0;

            foreach (var i in world.Enemies)
            {
                ObjectDesc desc = i.Value.ObjectDesc;

                if (desc == null || (desc.DisplayId ?? desc.ObjectId) != taunt.Item1)
                    continue;

                count++;
            }

            if (count == 0)
                return;

            if (count == 1 && taunt.Item2.final != null)
            {
                string[] arr = taunt.Item2.final;
                string msg = arr[rand.Next(0, arr.Length)];

                BroadcastMsg(msg);
            }
            else
            {
                string[] arr = taunt.Item2.numberOfEnemies;
                string msg = arr[rand.Next(0, arr.Length)];

                BroadcastMsg(msg.Replace("{COUNT}", count.ToString()));
            }
        }
    }
}