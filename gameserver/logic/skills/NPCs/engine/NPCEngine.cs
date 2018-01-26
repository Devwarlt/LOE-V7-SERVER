#region "using"

using System;
using gameserver.realm;
using System.Collections.Generic;

#endregion

namespace gameserver.logic.behaviors
{
		/** NPC Engine
		* Author: Warlt (LoESoft Games)
		*/
  
		public class NPCEngine : Behavior
		{
				// NPC read-only variables (declaration) 
				protected List<string> _playerWelcomeMessages { get; set; }
				protected List<string> _NPCWelcomeMessages { get; set; }
				protected bool _randomNPCWelcomeMessages { get; set; }
				protected List<string> _playerLeaveMessages { get; set; }
				protected List<string> _NPCLeaveMessages { get; set; }
				protected bool _randomNPCLeaveMessages { get; set; }
				protected int _range { get; set; }
				protected string _NPCName { get; set; } // internal set
                protected ChatManager chatManager;
				
				// Engine read-only variables
				protected string _PLAYER = "{PLAYER_NAME}";
				protected string _NPC = "{NPC_NAME}";
				protected DateTime _now => DateTime.Now; // get current time (real-time) 
				protected int _delay = 1000; // in milliseconds
				
				public NPCEngine(List<string> playerWelcomeMessages = null, List<string> NPCWelcomeMessages = null, bool randomNPCWelcomeMessages = true,
                    List<string> playerLeaveMessages = null, List<string> NPCLeaveMessages = null, bool randomNPCLeaveMessages = true, int range = 8)
				{
						_playerWelcomeMessages = playerWelcomeMessages != null ? playerWelcomeMessages : new List<string> { "hi", "hello", "hey", "good morning", "good afternoon", "good evening" };
						_NPCWelcomeMessages = NPCWelcomeMessages != null ? NPCWelcomeMessages : new List<string> { $"Welcome, **{_PLAYER}**!", $"Hail **{_PLAYER}**!" };
						_randomNPCWelcomeMessages = randomNPCWelcomeMessages;
						_playerLeaveMessages = playerLeaveMessages != null ? playerLeaveMessages : new List<string> { "bye", "see ya", "goodbye", "see you", "see you soon", "goodnight" };
						_NPCLeaveMessages = NPCLeaveMessages != null ? NPCLeaveMessages : new List<string> { "Farewell!", "Good bye then.", "Cheers!", "See you soon!" };
						_randomNPCLeaveMessages = randomNPCLeaveMessages;
						_range = range;
				}
				
				// first handler, initialize engine (declarations only)
				protected override void OnStateEntry(Entity npc, RealmTime time, ref object state)
				{
				    state = 0;
				}
				
				// duty loop
				protected override void TickCore(Entity npc, RealmTime time, ref object state)
				{
				    string playerMessage = string.Empty;
					var players = npc.GetNearestEntities(_range, null);
                    foreach (Entity player in players)
					{
					    if (player != null &&  chatManager.ChatData.ContainsKey(player.Name))
						{
						    foreach (Tuple<DateTime, string> messageInfo in chatManager.ChatData[player.Name])
							if (messageInfo.Item1.AddMilliseconds(- _delay) <= _now && _playerWelcomeMessages.Contains(messageInfo.Item2.ToLower()))
							{ // validate message handler
							    chatManager.ChatData[player.Name].Remove(messageInfo); // delete message to avoid duplicated check
								ProcessNPC(npc.Name);
								continue; // stop handler
								// process NPC welcome message here
							}
            		    }
        			}
    			}
    			
    			private void ProcessNPC(string name)
    			{
    					// TODO: implement declaration & handlers (might use dictionary with checker for auto process)
    			}
  		}
}