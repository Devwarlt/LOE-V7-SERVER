#region

using gameserver.networking.incoming;
using gameserver.realm;

#endregion

namespace gameserver.networking.handlers
{
    internal class CheckCreditsHandler : MessageHandlers<CHECKCREDITS>
    {
        public override MessageID ID => MessageID.CHECKCREDITS;

        protected override void HandleMessage(Client client, CHECKCREDITS message) => Handle(client);

        private void Handle(Client client)
        {
            client.Account.Flush();
            client.Account.Reload();
            client.Manager.Logic.AddPendingAction(t =>
            {
                client.Player.Credits = client.Player.Client.Account.Credits;
                client.Player.UpdateCount++;
            }, PendingPriority.Networking);
        }
    }
}