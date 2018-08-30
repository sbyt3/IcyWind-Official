﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace IcyWind.Core.Logic.Riot.Compression
{
    public class Deflate
    {
        public static byte[] Compress(byte[] input)
        {
            // Create the compressor with highest level of compression  
            Deflater compressor = new Deflater();
            compressor.SetLevel(Deflater.BEST_COMPRESSION);

            // Give the compressor the data to compress  
            compressor.SetInput(input);
            compressor.Finish();

            /* 
             * Create an expandable byte array to hold the compressed data. 
             * You cannot use an array that's the same size as the orginal because 
             * there is no guarantee that the compressed data will be smaller than 
             * the uncompressed data. 
             */
            MemoryStream bos = new MemoryStream(input.Length);

            // Compress the data  
            byte[] buf = new byte[1024];
            while (!compressor.IsFinished)
            {
                int count = compressor.Deflate(buf);
                bos.Write(buf, 0, count);
            }

            // Get the compressed data  
            return bos.ToArray();
        }

        public static byte[] Uncompress(byte[] input)
        {
            Inflater decompressor = new Inflater(true);
            decompressor.SetInput(input);

            // Create an expandable byte array to hold the decompressed data  
            MemoryStream bos = new MemoryStream(input.Length);

            // Decompress the data  
            byte[] buf = new byte[1024];
            while (!decompressor.IsFinished)
            {
                int count = decompressor.Inflate(buf);
                bos.Write(buf, 0, count);
            }

            // Get the decompressed data  
            return bos.ToArray();
        }
    }
}
