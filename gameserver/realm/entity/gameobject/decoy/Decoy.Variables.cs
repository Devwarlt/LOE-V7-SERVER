#region

using Mono.Game;
using System;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.realm.entity
{
    partial class Decoy
    {
        private static readonly Random rand = new Random();
        private readonly int duration;
        private readonly float speed;
        private Vector2 direction;
    }
}
