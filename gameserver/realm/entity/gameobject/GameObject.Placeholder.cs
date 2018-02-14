namespace gameserver.realm.entity
{
    internal class Placeholder : GameObject
    {
        public Placeholder(int life)
            : base(0x070f, life, true, true, false)
        {
            Size = 0;
        }
    }
}