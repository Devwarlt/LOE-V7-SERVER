namespace gameserver.realm.mapsetpiece
{
    internal class FrogKing : MapSetPiece
    {
        public override int Size => 5;

        public override void RenderSetPiece(World world, IntPoint pos)
        {
            Entity cube = Entity.Resolve(world.Manager, "Frog King");
            cube.Move(pos.X + 2.5f, pos.Y + 2.5f);
            world.EnterWorld(cube);
        }
    }
}