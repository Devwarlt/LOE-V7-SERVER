using LoESoft.GameServer.networking.incoming;

namespace LoESoft.GameServer.networking.handlers
{
    internal class TinkerQuestHandler : MessageHandlers<QUEST_REDEEM_RESPONSE>
    {
        public override MessageID ID => MessageID.QUEST_REDEEM_RESPONSE;

        protected override void HandleMessage(Client client, QUEST_REDEEM_RESPONSE message) => NotImplementedMessageHandler();
    }
}