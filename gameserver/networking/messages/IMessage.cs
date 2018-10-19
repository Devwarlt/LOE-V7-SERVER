using LoESoft.GameServer.networking.incoming;

namespace LoESoft.GameServer.networking
{
    internal interface IMessage
    {
        MessageID ID { get; }

        void Handle(Client client, IncomingMessage message);
    }
}