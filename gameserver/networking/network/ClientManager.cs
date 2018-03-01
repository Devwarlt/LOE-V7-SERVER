using System;
using System.Collections.Generic;

namespace LoESoft.GameServer.networking.network
{
    internal sealed class ClientManager
    {
        readonly Queue<Client> _pool;

        internal ClientManager(Int32 capacity)
        {
            _pool = new Queue<Client>(capacity);
        }

        internal Int32 Count
        { get { return _pool.Count; } }

        internal Client Pop()
        { using (TimedLock.Lock(_pool)) { return _pool.Dequeue(); } }

        internal void Push(Client client)
        {
            if (client == null)
                throw new ArgumentNullException("Clients added to a ClientManager cannot be null");

            using (TimedLock.Lock(_pool))
                if (!_pool.Contains(client))
                    _pool.Enqueue(client);
        }
    }
}