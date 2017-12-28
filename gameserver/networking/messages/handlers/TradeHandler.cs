#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class RequestTradeHandler : MessageHandlers<REQUESTTRADE>
    {
        public override MessageID ID => MessageID.REQUESTTRADE;

        protected override void HandleMessage(Client client, REQUESTTRADE message) => client.Manager.Logic.AddPendingAction(t => client.Player.RequestTrade(t, message));
    }

    internal class ChangeTradeHandler : MessageHandlers<CHANGETRADE>
    {
        public override MessageID ID => MessageID.CHANGETRADE;

        protected override void HandleMessage(Client client, CHANGETRADE message) => client.Manager.Logic.AddPendingAction(t => client.Player.ChangeTrade(t, message));
    }

    internal class AcceptTradeHandler : MessageHandlers<ACCEPTTRADE>
    {
        public override MessageID ID => MessageID.ACCEPTTRADE;

        protected override void HandleMessage(Client client, ACCEPTTRADE message) => client.Manager.Logic.AddPendingAction(t => client.Player.AcceptTrade(t, message));
    }

    internal class CancelTradeHandler : MessageHandlers<CANCELTRADE>
    {
        public override MessageID ID => MessageID.CANCELTRADE;

        protected override void HandleMessage(Client client, CANCELTRADE message) => client.Manager.Logic.AddPendingAction(t => client.Player.CancelTrade(t, message));
    }
}