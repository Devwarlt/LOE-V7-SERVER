#region

using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic
{
    public partial class TaskManager
    {
        public class TaskProgress
        {
            public List<TaskHistory> TaskHistory { get; private set; }

            public TaskProgress(
                List<TaskHistory> TaskHistory
                )
            {
                this.TaskHistory = TaskHistory;
            }
        }

        public class TaskHistory
        {
            public int TaskID { get; private set; }
            public bool TaskCompleted { get; private set; }
            public List<ushort> EntitiesID { get; private set; }
            public List<int> EntitiesAmount { get; private set; }

            public TaskHistory(
                int TaskID,
                bool TaskCompleted,
                List<ushort> EntitiesID,
                List<int> EntitiesAmount
                )
            {
                this.TaskID = TaskID;
                this.TaskCompleted = TaskCompleted;
                this.EntitiesID = EntitiesID;
                this.EntitiesAmount = EntitiesAmount;
            }
        }

        public class TaskData
        {
            public string Name { get; private set; }
            public List<ushort> EntitiesTarget { get; private set; }
            public List<int> EntitiesAmount { get; private set; }
            public List<ushort> Rewards { get; private set; }
            public List<int> RewardsAmount { get; private set; }

            public TaskData(
                string Name,
                EntitiesData EntitiesData,
                RewardData RewardData
                )
            {
                this.Name = Name;
                EntitiesTarget = EntitiesData.EntitiesID;
                EntitiesAmount = EntitiesData.EntitiesAmount;
                Rewards = RewardData.Rewards;
                RewardsAmount = RewardData.RewardsAmount;
            }
        }

        public class EntitiesData
        {
            public List<ushort> EntitiesID { get; set; }
            public List<int> EntitiesAmount { get; set; }

            public EntitiesData(
                List<ushort> EntitiesID,
                List<int> EntitiesAmount
                )
            {
                this.EntitiesID = EntitiesID;
                this.EntitiesAmount = EntitiesAmount;
            }
        }

        public class RewardData
        {
            public List<ushort> Rewards { get; set; }
            public List<int> RewardsAmount { get; set; }

            public RewardData(
                List<ushort> Rewards,
                List<int> RewardsAmount
                )
            {
                this.Rewards = Rewards;
                this.RewardsAmount = RewardsAmount;
            }
        }
    }
}