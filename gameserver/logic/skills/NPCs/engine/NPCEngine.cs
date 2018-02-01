#region

using System;
using gameserver.realm;
using System.Collections.Generic;
using gameserver.realm.entity.player;
using gameserver.networking.outgoing;
using System.Threading.Tasks;

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
        public override void Welcome(Player player) => Callback(player, $"Hello {player.Name}! I'm Gazer, how can I help you?");

        public override void Leave(Player player, bool polite) => Callback(player, polite ? _NPCLeaveMessage.Replace("{PLAYER}", player.Name) : "How rude!");

        protected override void SetStars() => _NPCStars = 70;
    }
    #endregion

    public abstract class NPC
    {
        protected List<string> _playersCache { get; set; }
        protected Entity _NPC { get; set; }
        protected string _NPCLeaveMessage { get; set; }
        protected int _NPCStars { get; set; }

        public void Config(
            Entity NPC,
            List<string> NPCLeaveMessages,
            bool randomNPCLeaveMessages
            )
        {
            _playersCache = new List<string>();
            _NPC = NPC;
            _NPCLeaveMessage = randomNPCLeaveMessages ? NPCLeaveMessages[new Random().Next(0, NPCLeaveMessages.Count)] : NPCLeaveMessages[0];
        }

        public void NoRepeat(Player player) => Callback(player, $"I'm already talking to you, {player.Name}...");

        public List<string> ReturnPlayersCache() => _playersCache;

        public void AddPlayer(Player player) => _playersCache.Add(player.Name);

        public void RemovePlayer(Player player) => _playersCache.Remove(player.Name);

        public abstract void Welcome(Player player);

        public abstract void Leave(Player player, bool polite);

        protected abstract void SetStars();

        // send a private message to specific player (idea by Sebafra)
        protected void Callback(
            Player player,
            string message
            )
        {
            if (player == null) return;
            TEXT _text = new TEXT();
            _text.ObjectId = _NPC.Id;
            _text.BubbleTime = 10;
            _text.Stars = _NPCStars;
            _text.Name = _NPC.Name;
            _text.Admin = 0;
            _text.Recipient = player.Name;
            _text.Text = message.ToSafeText();
            _text.CleanText = "";
            _text.NameColor = _text.TextColor = 0x123456;
            player.Client.SendMessage(_text);
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
        protected double _range { get; set; }
        protected string _NPCName { get; set; } // internal set
        protected NPC _NPC { get; set; }

        // Engine read-only variables
        protected DateTime _now => DateTime.Now; // get current time (real-time) 
        protected int _delay = 1000; // in milliseconds

        public NPCEngine(
            List<string> playerWelcomeMessages = null,
            List<string> playerLeaveMessages = null,
            List<string> NPCLeaveMessages = null,
            bool randomNPCLeaveMessages = true,
            double range = 8.00
            )
        {
            _playerWelcomeMessages = playerWelcomeMessages != null ? playerWelcomeMessages : new List<string> { "hi", "hello", "hey", "good morning", "good afternoon", "good evening" };
            _playerLeaveMessages = playerLeaveMessages != null ? playerLeaveMessages : new List<string> { "bye", "see ya", "goodbye", "see you", "see you soon", "goodnight" };
            _NPCLeaveMessages = NPCLeaveMessages != null ? NPCLeaveMessages : new List<string> { "Farewell, {PLAYER}!", "Good bye, then...", "Cheers!", "See you soon, {PLAYER}!" };
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
            _NPC = NPCDatabase.ContainsKey(npc.Name) ? NPCDatabase[npc.Name] : null;
            _NPC.Config(npc, _NPCLeaveMessages, _randomNPCLeaveMessages);
            state = 0;
        }

        // duty loop
        protected override void TickCore(
            Entity npc,
            RealmTime time,
            ref object state
            )
        {
            if (_NPC == null) return;
            Parallel.ForEach(npc.GetNearestEntities(_range * _range, null), entity =>
            {
                Player player;
                if (entity is Player)
                    player = entity as Player;
                else
                    return;
                try
                {
                    if (ChatManager.ChatDataCache.ContainsKey(player.Name))
                    {
                        foreach (Tuple<DateTime, string> messageInfo in ChatManager.ChatDataCache[player.Name])
                        {
                            if (_NPC.ReturnPlayersCache().Contains(player.Name))
                            {
                                if (messageInfo.Item1.AddMilliseconds(-_delay) <= _now && _playerWelcomeMessages.Contains(messageInfo.Item2.ToLower()) && npc.Dist(player) <= _range)
                                {
                                    _NPC.NoRepeat(player); // if player keep repeating welcome message >.>
                                    ChatManager.ChatDataCache[player.Name].Remove(messageInfo); // auto clean chat data cache
                                }
                                if (npc.Dist(player) > _range || _playerLeaveMessages.Contains(messageInfo.Item2.ToLower()))
                                {
                                    _NPC.RemovePlayer(player);
                                    _NPC.Leave(player, !(npc.Dist(player) > _range));
                                }
                            }
                            else
                            {
                                if (messageInfo.Item1.AddMilliseconds(-_delay) <= _now && _playerWelcomeMessages.Contains(messageInfo.Item2.ToLower()) && npc.Dist(player) <= _range)
                                {
                                    _NPC.Welcome(player);
                                    _NPC.AddPlayer(player);
                                    ChatManager.ChatDataCache[player.Name].Remove(messageInfo); // auto clean chat data cache
                                }
                            }
                        }
                    }
                }
                catch (InvalidOperationException) { } // collection can be updated, so new handler exception for it
            });
        }
    }
}