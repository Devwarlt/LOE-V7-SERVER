using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LoESoft.GameServer.networking.experimental
{
    internal sealed class ESocketAsyncEventArgsManager
    {
        internal Stack<SocketAsyncEventArgs> pool;

        internal ESocketAsyncEventArgsManager(Int32 capacity)
        {
            pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        internal Int32 Count
        { get { return pool.Count; } }

        internal SocketAsyncEventArgs Pop()
        { using (TimedLock.Lock(pool)) { return pool.Pop(); } }

        internal void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");

            using (TimedLock.Lock(pool))
                if (!pool.Contains(item))
                    pool.Push(item);
        }
    }
}
