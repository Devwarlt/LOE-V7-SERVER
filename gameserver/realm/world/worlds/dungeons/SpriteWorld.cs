#region

using System;

#endregion

namespace gameserver.realm.world
{
    public class SpriteWorld : World
    {
        private Random r = new Random();

        public SpriteWorld()
        {
            Name = "Sprite World";
            ClientWorldName = "dungeons.Sprite_World";
            Background = 0;
            Difficulty = 2;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap($"dungeons.sprite_world.Sprite_World_{r.Next(1, 5).ToString()}", MapType.Json);
        }
    }
}