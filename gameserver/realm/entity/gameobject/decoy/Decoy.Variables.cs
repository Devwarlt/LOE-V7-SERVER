#region

using LoESoft.GameServer.realm.entity.player;
using Mono.Game;
using System;

#endregion

namespace LoESoft.GameServer.realm.entity
{
    partial class Decoy
    {
        private static readonly Random rand = new Random();
        private readonly int duration;
        private readonly Player player;
        private readonly float speed;
        private Vector2 direction;
    }
}