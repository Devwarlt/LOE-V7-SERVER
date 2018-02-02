﻿#region

using gameserver.logic.behaviors;
using gameserver.realm;
using gameserver.realm.entity.npc;
using gameserver.realm.entity.npc.npcs;
using System.Collections.Generic;

#endregion

namespace gameserver.logic
{
    public class NPCs
    {
        // Declare NPCs here bellow
        protected NPC Gazer { get; set; }

        // Do not change it!
        public static readonly Dictionary<string, NPC> Database = new Dictionary<string, NPC>();

        public void Initialize(RealmManager manager)
        {
            // Initialize NPCs bellow
            Gazer = new Gazer();

            // Add NPCs into database
            Database.Add("NPC Gazer", Gazer);

            // Process all NPCs creating new instance for each one
            foreach (KeyValuePair<string, NPC> i in Database)
                i.Value.Config(Entity.Resolve(manager, i.Key), null, false);
        }
    }

    partial class BehaviorDb
    {
        private _ NPCCache = () => Behav()
            .Init("NPC Gazer", new State(new NPCEngine(NPCStars: 70)))
        ;
    }
}