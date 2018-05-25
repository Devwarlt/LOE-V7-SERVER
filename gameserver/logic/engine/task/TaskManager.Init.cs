#region

using LoESoft.GameServer.realm.entity.player;
using System;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic
{
    /* Task System (LoESoft Games)
     * Author: DV
     * */
    public partial class TaskManager
    {
        protected Player player { get; set; }
        public TaskProgress taskProgress { get; internal set; }

        public TaskManager(
            Player player
            )
        {
            this.player = player;
            //taskProgress = Deserialize(player.Client.Character.TaskStats);
        }

        /** Data format sample:
         * TASK_1:TASK_1_COMPLETED/ENTITY_1_ID,ENTITY_1_AMOUNT;ENTITY_2_ID,ENTITY_2_AMOUNT;...|TASK_2:TASK_2_COMPLETED/ENTITY_1_ID,ENTITY_1_AMOUNT;ENTITY_2_ID,ENTITY_2_AMOUNT;...
         */
        private TaskProgress Deserialize(string data)
        {
            if (data.Length == 0 || data == string.Empty || data == "" || data == " " || data == null)
            {
                List<TaskHistory> processedData = new List<TaskHistory>();
                foreach (KeyValuePair<int, TaskData> i in Database)
                    processedData.Add(new TaskHistory(i.Key, false, i.Value.EntitiesTarget, i.Value.EntitiesAmount));
                return new TaskProgress(processedData);
            }
            return null;
        }

        public string Serialize(TaskProgress data)
        {
            List<Tuple<int, List<ushort>, List<int>>> taskIDs = new List<Tuple<int, List<ushort>, List<int>>>();
            foreach (TaskHistory i in data.TaskHistory)
                taskIDs.Add(Tuple.Create(i.TaskID, i.EntitiesID, i.EntitiesAmount));
            List<Tuple<int, string>> serializeEntities = new List<Tuple<int, string>>();
            foreach (TaskHistory i in data.TaskHistory)
                for (int j = 0; j < i.EntitiesID.Count; j++)
                    serializeEntities.Add(Tuple.Create(i.TaskID, $"{(j == 0 ? $"{i.EntitiesID[j]}" : $"{serializeEntities[j - 1].Item2}{i.EntitiesID[j]}")}{i.EntitiesAmount[j]}{(j + 1 != i.EntitiesID.Count ? ";" : "|")}"));
            return serializeEntities[serializeEntities.Count - 1].Item2;
        }

        public void ProcessTask(int id)
        {
            if (!Database.ContainsKey(id))
                return;
        }
    }
}