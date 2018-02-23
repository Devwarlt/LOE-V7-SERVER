#region

using LoESoft.Core.models;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.logic.transitions
{
    public class DamageTakenTransition : Transition, INonSkippableState
    {
        // State storage: none
        private string Name { get; set; }
        private int InvalidHP { get; set; }
        private int Damage { get; set; }

        public DamageTakenTransition(
            int Damage,
            string TargetState
            ) : base(TargetState)
        {
            this.Damage = Damage;
            Skip = false;
            DoneAction = false;
            DoneStorage = false;
        }

        // Exclusive for new interface: INonSkipState
        public Enemy Enemy { get; set; }
        public bool DoneAction { get; set; }
        public bool DoneStorage { get; set; }
        public bool Skip { get; set; }
        public int StoreHP { get; set; }

        public void ManageHP(int stored, int threshold)
        {
            Log.Info(
                string.Format(
                "New INonSkippable interface usage!\n"
                + "\t[\n"
                + "\t\tEntity: {0},\n"
                + "\t\tStoredHP: {1},\n"
                + "\t\tThreshold: {2},\n"
                + "\t\tStoredHP (new): {3},\n"
                + "\t\tIs invalid HP? {4} ({5})\n"
                + "\t]\n"
                , Name, stored, threshold, stored - threshold, InvalidHP == stored - threshold, InvalidHP
            ));

            StoreHP = stored - threshold;
            DoneStorage = true;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            if (!DoneAction)
            {
                Enemy = host as Enemy;
                Name = Enemy.Name;
                StoreHP = Enemy.HP;
                DoneAction = true;
            }

            if (Skip)
            {
                InvalidHP = Enemy.HP;
                ManageHP(StoreHP, Damage);
                return true;
            }
            else
            {
                int damageSoFar = 0;

                foreach (var i in (host as Enemy).DamageCounter.GetPlayerData())
                    damageSoFar += i.Item2;

                if (damageSoFar >= Damage)
                    return true;
                return false;
            }
        }
    }
}
