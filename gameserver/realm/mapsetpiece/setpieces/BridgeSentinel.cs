﻿namespace LoESoft.GameServer.realm.mapsetpiece
{
    internal class BridgeSentinel : MapSetPiece
    {
        public override int Size => 5;

        public override void RenderSetPiece(World world, IntPoint pos)
        {
            Entity boss = Entity.Resolve("shtrs Bridge Sentinel");
            boss.Move(pos.X, pos.Y);

            Entity chestSpawner = Entity.Resolve("shtrs encounterchestspawner");
            chestSpawner.Move(pos.X, pos.Y + 5f);

            Entity blobombSpawner1 = Entity.Resolve("shtrs blobomb maker");
            blobombSpawner1.Move(pos.X, pos.Y + 5f);

            Entity blobombSpawner2 = Entity.Resolve("shtrs blobomb maker");
            blobombSpawner2.Move(pos.X + 5f, pos.Y + 5f);

            Entity blobombSpawner3 = Entity.Resolve("shtrs blobomb maker");
            blobombSpawner3.Move(pos.X - 5f, pos.Y + 5f);

            world.EnterWorld(boss);

            world.EnterWorld(chestSpawner);

            world.EnterWorld(blobombSpawner1);
            world.EnterWorld(blobombSpawner2);
            world.EnterWorld(blobombSpawner3);
        }
    }
}