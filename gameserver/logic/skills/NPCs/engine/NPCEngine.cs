namespace gameserver.logic.behaviors {
  /** NPC Engine
  * Author: Warlt (LoESoft Games)
  */
  
  public class NPCEngine : Behavior {
    // NPC read-only variables (declaration) 
    protected List<string> _playerWelcomeMessages { get; set; }
    protected List<string> _NPCWelcomeMessages { get; set; }
    protected bool _randomNPCWelcomeMessages { get; set; }
    protected List<string> _playerLeaveMessages { get; set; }
    protected List<string> _NPCLeaveMessages { get; set; }
    protected bool _randomNPCLeaveMessages { get; set; }
    protected int _range { get; set; }
    protected string _NPCName { get; set; } // internal set
    
    // Engine read-only variables
    protected string _PLAYER = "{PLAYER_NAME}";
    protected string _NPC = "{NPC_NAME}";
    protected DateTime _now => DateTime.Now; // get current time (real-time) 
    protected int _delay = 1000; // in milliseconds
    protected string _noob = "Devwarlt";
    
    public NPCEngine(
      List<string> playerWelcomeMessages = new List<string> { "hi", "hello", "hey", "good morning", "good afternoon", "good evening" },
      List<string> NPCWelcomeMessages = new List<string> { $"Welcome, **{_PLAYER}**!", $"Hail **{_PLAYER}**!" },
      bool randomNPCWelcomeMessages = true,
      List<string> playerLeaveMessages = new List<string> { "bye", "see ya", "goodbye", "see you", "see you soon", "goodnight" },
      List<string> NPCLeaveMessages = new List<string> { "Farewell!", "Good bye then.", "Cheers!", "See you soon!" },
      bool randomNPCLeaveMessages = true,
      int range = 8
      ) {
        _playerWelcomeMessages = playerWelcomeMessages;
        _NPCWelcomeMessages = NPCWelcomeMessages;
        _randomNPCWelcomeMessages = randomNPCWelcomeMessages; 
        _playerLeaveMessages = playerLeaveMessages;
        _NPCLeaveMessages = NPCLeaveMessages;
        _randomNPCLeaveMessages = randomNPCLeaveMessages; 
        _range = range;
    }
    
    // first handler, initialize engine (declarations only)
    protected override void OnStateEntry(Entity npc, RealmTime time, ref object state) {
        state = 0;
    }
    
    // duty loop
    protected override void TickCore(Entity npc, RealmTime time, ref object state) {
        string playerMessage = string.Empty;
        List<Entity> players = npc.GetNearestEntities(_range, null).ToList();
        foreach (Entity player in players) {
            if (player != null && ChatManager.ChatData.ContainsKey(player.Name)) {
                foreach (Tuple<DateTime, string> messageInfo in ChatManager.ChatData[player.Name])
                    if (messageInfo.Item1.AddMilliseconds(- delay) <= _now && _playerWelcomeMessages.Contains(messageInfo.Item2.ToLower())) { // validate message handler
                        ChatManager.ChatData[player.Name].Remove(messageInfo); // delete message to avoid duplicated check
                        ProcessNPC(npc.Name);
                        continue; // stop handler
                        // process NPC welcome message here
                    }
            }
        }
    }
    
    private void ProcessNPC(string name) {
        // TODO: implement declaration & handlers (might use dictionary with checker for auto process)
    }
  }
}

// this one is used to send data for NPCs (internal data traffic)
#region "ChatManager - extra feature"
  public Dictionary<string, List<Tuple<DateTime, string>>> ChatData = new Dictionary<string, List<Tuple<DateTime, string>>>();
  
  public void Say(Player src, string text) {
      if (!ChatData.ContainsKey(src.Name))
          ChatData.Add(src.Name, new List<Tuple<DateTime, string>> { Tuple.Create(DateTime.Now, text) });
      else
          ChatData[src.Name].Add(Tuple.Create(DateTime.Now, text));
      // the rest of code bellow (not gonna copy it here)
  }
#endregion