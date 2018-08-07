using LoESoft.Core.assets.itemdata;
using System.Collections.Generic;

namespace LoESoft.GameServer.realm.entity.gameobject
{
    public interface ICorpse
    {
        List<GameItem> Items { get; }
    }

    public class Corpse : Entity, ICorpse
    {
        /// <summary>
        /// Idea by ǝuoɹp.
        /// </summary>
        [System.Flags]
        public enum Phases
        {
            BrandNew = -1,
            Private = 0,
            Public = 1,
            Decay = 2
        }

        public string ItemId { get; set; }
        public Object CorpseData { get; set; }
        public int Capacity { get; set; }
        public List<DecayData> DecayData { get; set; }
        public List<GameItem> Items { get; set; }
        public Phases CurrentPhase { get; set; }
        public string CorpseOwner { get; set; }
        private ElapsedCorpseTime DecayTime { get; set; }
        private string CurrentFile { get; set; }
        private uint CurrentIndex { get; set; }
        private bool ExportOnce { get; set; }

        public Corpse(string id)
        {
            ItemId = id;
            //CorpseData = GameServer.Manager.ItemDataManager.ItemDatas[id] as Object;
            Capacity = CorpseData.Capacity;
            DecayData = CorpseData.DecayData;
            Items = new List<GameItem>(Capacity);
            CurrentPhase = Phases.BrandNew;
            DecayTime = new ElapsedCorpseTime();
            CurrentFile = CorpseData.File;
            CurrentIndex = CorpseData.Index;
            ExportOnce = false;
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            /*if (!ExportOnce)
            {
                ExportOnce = true;
                stats[StatsType.CONTAINER_ITEM_DATA] = ItemDataAttribute.Export(CorpseData);
            }
            stats[StatsType.CONTAINER_PRIORITY] = (int)CurrentPhase;
            stats[StatsType.CONTAINER_OWNER] = CorpseOwner ?? "-1";*/
        }

        public override void Tick(RealmTime time)
        {
            switch (CurrentPhase)
            {
                case Phases.Private:
                    if (DecayTime.Measure >= DecayData[0].Time)
                    {
                        CurrentPhase = Phases.Public;
                        DecayTime = new ElapsedCorpseTime();
                        CurrentFile = DecayData[1].File;
                        CurrentIndex = DecayData[1].Index;
                    }
                    break;
                case Phases.Public:
                    if (DecayTime.Measure >= DecayData[1].Time)
                    {
                        CurrentPhase = Phases.Decay;
                        CorpseOwner = "-1";
                        DecayTime = new ElapsedCorpseTime();
                        CurrentFile = DecayData[2].File;
                        CurrentIndex = DecayData[2].Index;
                    }
                    break;
                case Phases.Decay:
                    if (DecayTime.Measure >= DecayData[2].Time)
                        Owner.LeaveWorld(this);
                    break;
                default:
                    if (DecayTime.Measure >= 15)
                    {
                        CurrentPhase = Phases.Private;
                        DecayTime = new ElapsedCorpseTime();
                        CurrentFile = DecayData[0].File;
                        CurrentIndex = DecayData[0].Index;
                    }
                    break;
            }

            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time) => false;
    }
}
