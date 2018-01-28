#region

using System;
using gameserver.realm;
using System.Collections.Generic;

#endregion

namespace gameserver.logic.behaviors
{
		/** NPC Engine (LoESoft Games)
		* Author: Warlt
		* Code Review: Sebafra
		*/
		
		public abstract class NPC
		{
				protected string _playerTargetName { get; set; }
				protected Entity _NPCEntity { get; set; }
				protected List<string> _NPCLeaveMessages { get; set; }
				protected bool _randomNPCLeaveMessages { get; set; }
				
				public void Config(
						string playerTargetName,
						Entity NPCEntity,
						List<strint> NPCLeaveMessages,
						bool randomNPCLeaveMessages
						)
				{
						_playerTargetName = playerTargetName;
						_NPCEntity = NPCEntity;
						_NPCLeaveMessages = NPCLeaveMessages;
						_randomNPCLeaveMessages = randomNPCLeaveMessages;
				}
				
				// this method get override
				public void Init() {}
						
				// we might use override methods for these kind of functions since NPCs gonna use several commands and extras.
				public void NPCCommands()
				{
						// TODO: add basic welcome messages proccessed here, if this void function isn't overrided by other NPC module.
				}
				
				public void NPCExtras()
				{
						// TODO: add basic extras (migrate: 'online' and 'uptime' algorithms from virtual Gazer) into this extra module.
				}
		}
  		
  		// Engine only
		public class NPCEngine : Behavior
		{
				// NPCs declaration
				public readonly Dictionary<string, NPC> NPCDatabase = new Dictionary<string, NPC>
				{
						// TODO: add new experimental NPC.
						{ "Gazer", new Gazer() }
				};
				
				// NPC read-only variables (declaration) 
				protected List<string> _playerWelcomeMessages { get; set; }
				protected List<string> _playerLeaveMessages { get; set; }
				protected List<string> _NPCLeaveMessages { get; set; }
				protected bool _randomNPCLeaveMessages { get; set; }
				protected int _range { get; set; }
				protected string _NPCName { get; set; } // internal set
				protected ChatManager _chatManager;
				
				// Engine read-only variables
				protected string _PLAYER = "{PLAYER_NAME}";
				protected string _NPC = "{NPC_NAME}";
				protected DateTime _now => DateTime.Now; // get current time (real-time) 
				protected int _delay = 1000; // in milliseconds
				
				public NPCEngine(
						List<string> playerWelcomeMessages = null,
						List<string> playerLeaveMessages = null,
						List<string> NPCLeaveMessages = null,
						bool randomNPCLeaveMessages = true,
						int range = 8
						)
				{
						_playerWelcomeMessages = playerWelcomeMessages != null ? playerWelcomeMessages : new List<string> { "hi", "hello", "hey", "good morning", "good afternoon", "good evening" };
						_playerLeaveMessages = playerLeaveMessages != null ? playerLeaveMessages : new List<string> { "bye", "see ya", "goodbye", "see you", "see you soon", "goodnight" };
						_NPCLeaveMessages = NPCLeaveMessages != null ? NPCLeaveMessages : new List<string> { "Farewell!", "Good bye, then...", "Cheers!", "See you soon!" };
						_randomNPCLeaveMessages = randomNPCLeaveMessages;
						_range = range;
				}
				
				// first handler, initialize engine (declarations only)
				protected override void OnStateEntry(
						Entity npc,
						RealmTime time,
						ref object state
						)
				{
				    state = 0;
				}
				
				// duty loop
				protected override void TickCore(
						Entity npc,
						RealmTime time,
						ref object state
						)
				{
						string playerMessage = string.Empty;
						IEnumerable<Entity> players = npc.GetNearestEntities(_range, null);
						foreach (Entity player in players)
						{
								if (player != null &&  _chatManager.ChatData.ContainsKey(player.Name))
								{
										foreach (Tuple<DateTime, string> messageInfo in _chatManager.ChatData[player.Name])
												if (messageInfo.Item1.AddMilliseconds(- _delay) <= _now && _playerWelcomeMessages.Contains(messageInfo.Item2.ToLower()))
												{ // validate message handler
														_chatManager.ChatData[player.Name].Remove(messageInfo); // delete message to avoid duplicated check
														ProcessNPC(npc, player.Name);
														continue; // stop handler
												}
								}
						}
				}
    			
    			private void ProcessNPC(
    					Entity npc,
    					string target
    					)
    			{
    					NPC thisNPC = NPCDatabase[npc.Name];
    					thisNPC.Config(target, npc, _NPCLeaveMessages, _randomNPCLeaveMessages);
    					thisNPC.Init(); // always initialize NPC
    			}
  		}
}