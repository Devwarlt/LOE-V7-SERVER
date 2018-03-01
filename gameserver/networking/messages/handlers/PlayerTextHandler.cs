﻿#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class PlayerTextHandler : MessageHandlers<PLAYERTEXT>
    {
        public override MessageID ID => MessageID.PLAYERTEXT;

        protected override void HandleMessage(Client client, PLAYERTEXT message)
        {
            Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;

                if (message.Text[0] == '/')
                    Program.Manager.Commands.Execute(client.Player, t, message.Text);
                else
                {
                    if (client.Player.Muted)
                    {
                        client.Player.SendInfo("{\"key\":\"server.muted\"}");
                        return;
                    }
                    if (!client.Player.NameChosen)
                    {
                        client.Player.SendInfo("{\"key\":\"server.must_be_named\"}");
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(message.Text))
                        Program.Manager.Chat.Say(client.Player, message.Text);
                    else
                        client.Player.SendInfo("{\"key\":\"server.invalid_chars\"}");
                }
            });
        }
    }
}