﻿#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class PetCommandHandler : MessageHandlers<ACTIVE_PET_UPDATE_REQUEST>
    {
        public override MessageID ID => MessageID.ACTIVE_PET_UPDATE_REQUEST;

        protected override void HandleMessage(Client client, ACTIVE_PET_UPDATE_REQUEST message) => NotImplementedMessageHandler();
    }
}