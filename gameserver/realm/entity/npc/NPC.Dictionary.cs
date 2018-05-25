#region

using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity.npc;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic
{
    public class NPCs
    {
        // Declare NPCs here bellow

        // Do not change it!
        public static readonly Dictionary<string, NPC> Database = new Dictionary<string, NPC>();

        public void Initialize(RealmManager manager)
        {
            // Initialize NPCs bellow

            // Add NPCs into database

            // Process all NPCs creating new instance for each one
            foreach (KeyValuePair<string, NPC> i in Database)
                i.Value.Config(Entity.Resolve(i.Key), null, false);
        }
    }

    partial class BehaviorDb
    {
        private _ NPCCache = () => Behav();
    }
}