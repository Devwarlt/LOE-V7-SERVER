﻿#region

using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.networking.handlers
{
    internal class EditAccountListHandler : MessageHandlers<EDITACCOUNTLIST>
    {
        public override MessageID ID => MessageID.EDITACCOUNTLIST;

        protected override void HandleMessage(Client client, EDITACCOUNTLIST message) => client.Manager.Logic.AddPendingAction(t => Handle(client, message));

        private void Handle(Client client, EDITACCOUNTLIST message)
        {
            if (client.Player.Owner == null)
                return;

            Player target;

            target = client.Player.Owner.GetEntity(message.ObjectId) is Player ? client.Player.Owner.GetEntity(message.ObjectId) as Player : null;

            if (target == null)
                return;

            if(client.Account.AccountId == target.AccountId)
            {
                SendFailure("You cannot do that with yourself.");
                return;
            }

            switch (message.AccountListId)
            {
                case ACCOUNTLIST.LOCKED_LIST_ID:
                    client.Account.Database.LockAccount(client.Account, int.Parse(target.AccountId));
                    break;
                case ACCOUNTLIST.IGNORED_LIST_ID:
                    client.Account.Database.IgnoreAccount(client.Account, int.Parse(target.AccountId));
                    break;
            }
        }
    }
}