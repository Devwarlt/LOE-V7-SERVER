#region

using System.Collections.Generic;

#endregion

namespace gameserver.realm.entity
{
    partial class Decoy
    {
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.TEX1_STAT] = player.Texture1;
            stats[StatsType.TEX2_STAT] = player.Texture2;
            base.ExportStats(stats);
        }
    }
}
