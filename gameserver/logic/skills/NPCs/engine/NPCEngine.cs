#region

using System;
using gameserver.realm;
using System.Collections.Generic;
using gameserver.realm.entity.player;
using gameserver.networking.outgoing;

#endregion

namespace gameserver.logic.behaviors
{
	/** NPC Engine (LoESoft Games)
	* Author: Warlt
	* Code Review: Sebafra
	*/
		
	// this part of code shall be migrated (other files path)
	#region "Experimental NPC"
	public class Gazer : NPC
	{
        protected override void Welcome()
		{
			_NPCStars = 70; // not sure
			string welcome = $"Hello {_player.Name}! I'm Gazer, how can I help you?";
			Callback(welcome);
		}
	}
	#endregion
	
	public abstract class NPC
	{
        protected Player _player { get; set; }
		protected Entity _NPC { get; set; }
		protected List<string> _NPCLeaveMessages { get; set; }
		protected bool _randomNPCLeaveMessages { get; set; }
		protected int _NPCStars { get; set; }
			
		public void Config(
			Player player,
			Entity NPC,
			List<string> NPCLeaveMessages,
			bool randomNPCLeaveMessages
			)
		{
			_player = player;
			_NPC = NPC;
			_NPCLeaveMessages = NPCLeaveMessages;
			_randomNPCLeaveMessages = randomNPCLeaveMessages;
		}
				
		public void Init() => Welcome(); // to player target (private message)
			
		// this method gets override (can be customized)
		protected abstract void Welcome();
				
		// send a private message to specific player (idea by Sebafra)
		public void Callback(
			string message
			)
		{
			// i'm not sure about this callback message as well
			TEXT _text = new TEXT();
			_text.ObjectId = _NPC.Id;
			_text.BubbleTime = 10;
			_text.Stars = _NPCStars;
			_text.Name = _NPC.Name;
			_text.Admin = 0;
			_text.Recipient = _player.Name;
			_text.Text = message.ToSafeText();
			_text.CleanText = "";
			_text.NameColor = _text.TextColor = 0x123456;
			_player.Client.SendMessage(_text);
        }
	}
  		
  	// Engine only (this engine is not part of Behavior engine, only use it to integrate both)
	public class NPCEngine : Behavior
	{
		// NPCs declaration
		public readonly Dictionary<string, NPC> NPCDatabase = new Dictionary<string, NPC>
		{
			// TODO: add new experimental NPC.
			{ "NPC Gazer", new Gazer() }
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
			foreach (Player player in players)
            {
                try
                {
                    if (player != null && ChatManager.ChatDataCache.ContainsKey(player.Name))
                    {
                        foreach (Tuple<DateTime, string> messageInfo in ChatManager.ChatDataCache[player.Name])
                        {
                            if (messageInfo.Item1.AddMilliseconds(- _delay) <= _now && _playerWelcomeMessages.Contains(messageInfo.Item2.ToLower()))
                                ProcessNPC(npc, player);
                            // auto clean chat data cache
                            ChatManager.ChatDataCache[player.Name].Remove(messageInfo);
                        }
                    }
                } catch (InvalidOperationException) { } // collection can be updated, so new handler exception for it
            }
		}

    	private void ProcessNPC(
    		Entity npc,
    		Player player
    		)
    	{
            NPC thisNPC = NPCDatabase[npc.Name];
            thisNPC.Config(player, npc, _NPCLeaveMessages, _randomNPCLeaveMessages);
            thisNPC.Init(); // always initialize NPC
        }
  	}
}