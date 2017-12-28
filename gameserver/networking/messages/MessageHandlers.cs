using log4net;
using gameserver.networking.incoming;
using gameserver.realm;

namespace gameserver.networking
{
    internal abstract class MessageHandlers<T> : IMessage where T : IncomingMessage
    {
        protected ILog log4net;

        private Client client;

        public Client Client => client;
        public RealmManager Manager => client.Manager;

        public abstract MessageID ID { get; }

        public MessageHandlers() { log4net = LogManager.GetLogger(GetType()); }

        protected abstract void HandleMessage(Client client, T message);

        public void Handle(Client client, IncomingMessage message)
        {
            this.client = client;
            HandleMessage(client, (T) message);
        }

        protected void SendFailure(string text) => client.SendMessage(new FAILURE {ErrorId = 0, ErrorDescription = text});

        public void NotImplementedMessageHandler() { return; }
    }
}