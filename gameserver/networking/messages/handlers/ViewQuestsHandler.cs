#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class ViewQuestsHandler : MessageHandlers<QUEST_FETCH_ASK>
    {
        public override MessageID ID => MessageID.QUEST_FETCH_ASK;

        protected override void HandleMessage(Client client, QUEST_FETCH_ASK packet) => NotImplementedMessageHandler();
    }
}