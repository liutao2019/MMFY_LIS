using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace dcl.common
{
    public class Compression
    {
        public static byte[] Compress(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress);
            gZipStream.Write(data, 0, data.Length);

            gZipStream.Close();

            return stream.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
            byte[] bytes = new byte[4096];
            int n;
            while ((n = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                stream.Write(bytes, 0, n);
            }

            gZipStream.Close();
            return stream.ToArray();
        }


        /// <summary>
        /// 压缩字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DeflateData(byte[] data)
        {
            if (data == null) return null;

            byte[] buffer = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (DeflateStream inflateStream = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    inflateStream.Write(data, 0, data.Length);
                }

                stream.Seek(0, SeekOrigin.Begin);

                int length = Convert.ToInt32(stream.Length);
                buffer = new byte[length];
                stream.Read(buffer, 0, length);
            }

            return buffer;
        }



        /// <summary>
        /// 解压压缩数据
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public static byte[] InflateData(byte[] compressedData)
        {
            if (compressedData == null) return null;
            int deflen = compressedData.Length * 2;
            byte[] buffer = null;

            using (MemoryStream stream = new MemoryStream(compressedData))
            {
                using (DeflateStream inflatestream = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    using (MemoryStream uncompressedstream = new MemoryStream())
                    {
                        using (BinaryWriter writer = new BinaryWriter(uncompressedstream))
                        {
                            int offset = 0;
                            while (true)
                            {
                                byte[] tempbuffer = new byte[deflen];

                                int bytesread = inflatestream.Read(tempbuffer, offset, deflen);

                                writer.Write(tempbuffer, 0, bytesread);

                                if (bytesread < deflen || bytesread == 0) break;
                            }
                            uncompressedstream.Seek(0, SeekOrigin.Begin);
                            buffer = uncompressedstream.ToArray();
                        }
                    }
                }
            }

            return buffer;
        }
    }
}
