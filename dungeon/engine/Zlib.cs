using System;
using System.IO;
using System.IO.Compression;

namespace dungeon.engine
{
    public static class Zlib
    {
        static uint ADLER32(byte[] data)
        {
            const uint MODULO = 0xfff1;
            uint A = 1, B = 0;
            for (int i = 0; i < data.Length; i++)
            {
                A = (A + data[i]) % MODULO;
                B = (B + A) % MODULO;
            }
            return (B << 16) | A;
        }

        public static byte[] Compress(byte[] buffer)
        {
            byte[] comp;
            using (var output = new MemoryStream())
            {
                using (var deflate = new DeflateStream(output, CompressionMode.Compress))
                    deflate.Write(buffer, 0, buffer.Length);
                comp = output.ToArray();
            }

            // Refer to http://www.ietf.org/rfc/rfc1950.txt for zlib format
            const byte CM = 8;
            const byte CINFO = 7;
            const byte CMF = CM | (CINFO << 4);
            const byte FLG = 0xDA;

            byte[] result = new byte[comp.Length + 6];
            result[0] = CMF;
            result[1] = FLG;
            Buffer.BlockCopy(comp, 0, result, 2, comp.Length);

            uint cksum = ADLER32(buffer);
            var index = result.Length - 4;
            result[index++] = (byte)(cksum >> 24);
            result[index++] = (byte)(cksum >> 16);
            result[index++] = (byte)(cksum >> 8);
            result[index++] = (byte)(cksum >> 0);

            return result;
        }
    }
}