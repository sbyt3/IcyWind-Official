using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.Riot.Compression
{
    public static class Gzip
    {
        public static byte[] Decompress(byte[] data, int bufferSize = 2048)
        {
            using (var stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                var buffer = new byte[bufferSize];

                using (var memory = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, bufferSize);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static byte[] Compress(byte[] data)
        {
            var mem = new MemoryStream();
            using (var stream = new GZipStream(mem, CompressionMode.Compress))
            {
                stream.Write(data, 0, data.Length);

            }
            return mem.ToArray();
        }
    }
}
