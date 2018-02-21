#region

using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic
{
    public partial class TaskManager
    {
        public static readonly Dictionary<int, TaskData> Database = new Dictionary<int, TaskData>
        {
            #region "[Task: Scorpion Exterminator]"
            // ID: 0
            // Kill:
            //  - 100x Scorpion Queens;
            // Reward:
            //  - 1x Sword of Acclaim.
            {
                0,
                new TaskData(
                    Name: "Scorpion Exterminator",
                    EntitiesData: new EntitiesData(
                        EntitiesID: new List<ushort> { 0x604 },
                        EntitiesAmount: new List<int> { 100 }
                        ),
                    RewardData: new RewardData(
                        Rewards: new List<ushort> { 0xb0b },
                        RewardsAmount: new List<int> { 1 }
                        )
                    )
            }
            #endregion
        };
    }
}