#region

using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            stats[StatsType.ACCOUNT_ID_STAT] = AccountId;
            stats[StatsType.NAME_STAT] = Name;
            stats[StatsType.EXP_STAT] = Experience - GetLevelExperience(Level);
            stats[StatsType.NEXT_LEVEL_EXP_STAT] = ExperienceGoal - GetLevelExperience(Level);
            stats[StatsType.LEVEL_STAT] = Level;            
            stats[StatsType.CURR_FAME_STAT] = 0;
            stats[StatsType.NEXT_CLASS_QUEST_FAME_STAT] = 0;
            stats[StatsType.NUM_STARS_STAT] = Stars;
            stats[StatsType.GUILD_NAME_STAT] = Guild;
            stats[StatsType.GUILD_RANK_STAT] = GuildRank;
            stats[StatsType.CREDITS_STAT] = Credits;
            stats[StatsType.NAME_CHOSEN_STAT] = NameChosen ? 1 : 0;
            stats[StatsType.GLOW_COLOR_STAT] = Glowing ? 1 : 0;
            stats[StatsType.HP_STAT] = HP;
            stats[StatsType.MP_STAT] = MP;
            stats[StatsType.INVENTORY_0_STAT] = Inventory[0]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_1_STAT] = Inventory[1]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_2_STAT] = Inventory[2]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_3_STAT] = Inventory[3]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_4_STAT] = Inventory[4]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_5_STAT] = Inventory[5]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_6_STAT] = Inventory[6]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_7_STAT] = Inventory[7]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_8_STAT] = Inventory[8]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_9_STAT] = Inventory[9]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_10_STAT] = Inventory[10]?.ObjectType ?? -1;
            stats[StatsType.INVENTORY_11_STAT] = Inventory[11]?.ObjectType ?? -1;
            stats[StatsType.TEXTURE_STAT] = PlayerSkin;
            stats[StatsType.ACCOUNT_TYPE] = AccountType;
            stats[StatsType.ADMIN] = Admin;
        }
    }
}
