using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LoESoft.GameServer.networking.experimental
{
    internal sealed class BufferManager
    {
        internal Int32 totalBytesInBufferBlock;
        internal byte[] bufferBlock;
        internal Stack<int> freeIndexPool;
        internal Int32 currentIndex;
        internal Int32 bufferBytesAllocatedForEachSaea;

        public BufferManager(Int32 totalBytes, Int32 totalBufferBytesInEachSaeaObject)
        {
            totalBytesInBufferBlock = totalBytes;
            currentIndex = 0;
            bufferBytesAllocatedForEachSaea = totalBufferBytesInEachSaeaObject;
            freeIndexPool = new Stack<int>();
        }

        internal void InitBuffer() =>
            bufferBlock = new byte[totalBytesInBufferBlock];

        internal bool SetBuffer(SocketAsyncEventArgs args)
        {

            if (freeIndexPool.Count > 0)
                args.SetBuffer(bufferBlock, freeIndexPool.Pop(), bufferBytesAllocatedForEachSaea);
            else
            {
                if ((totalBytesInBufferBlock - bufferBytesAllocatedForEachSaea) < currentIndex)
                    return false;

                args.SetBuffer(bufferBlock, currentIndex, bufferBytesAllocatedForEachSaea);
                currentIndex += bufferBytesAllocatedForEachSaea;
            }
            return true;
        }

        internal void FreeBuffer(SocketAsyncEventArgs args)
        {
            freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
