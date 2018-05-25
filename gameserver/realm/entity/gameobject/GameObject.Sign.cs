namespace LoESoft.GameServer.realm.entity
{
    partial class Sign : GameObject
    {
        public Sign(int objType)
            : base(objType, null, true, false, false) { }

        public override bool HitByProjectile(Projectile projectile, RealmTime time) => false;
    }
}