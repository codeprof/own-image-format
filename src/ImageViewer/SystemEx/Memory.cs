using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.

namespace SystemEx
{
    /// <summary>
    /// Provides methods to compress/decompress and convert memory buffers
    /// </summary>
    public static class Memory
    {

        /// <summary>
        /// converts a byte buffer to a hex string
        /// </summary>
        public static string ByteArrayToHexString(byte[] bytes)
        {
            string hexString = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                string str = Convert.ToString(bytes[i], 16);
                if (str.Length == 1)
                {
                    str = "0" + str;
                }
                hexString += str;
            }
            return hexString;
        }

        /// <summary>
        /// converts a hex string to a byte buffer
        /// </summary>
        public static byte[] HexStringToByteArray(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// converts a string as UTF8 to a byte[] buffer
        /// </summary>
        public static byte[] ConvertUTF8StringToByteBuffer(string sText)
        {
            return Encoding.UTF8.GetBytes(sText);
        }

        /// <summary>
        /// converts a string as ASCII to a byte[] buffer
        /// </summary>
        public static byte[] ConvertASCIIStringToByteBuffer(string sText)
        {
            return Encoding.ASCII.GetBytes(sText);
        }

        /// <summary>
        /// converts a string as unicode to a byte[] buffer
        /// </summary>
        public static byte[] ConvertUnicodeStringToByteBuffer(string sText)
        {
            return Encoding.Unicode.GetBytes(sText);
        }       
        
        /// <summary>
        /// Compresses the buffer
        /// </summary>
        public static byte[] CompressBuffer(ref byte[] buffer)
        {
            byte[] memResult = null;
            /*try
            {
            */
            MemoryStream memStream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(memStream, CompressionMode.Compress, true);
            gZipStream.Write(buffer, 0, buffer.Length);
            memStream.Position = 0;
            byte[] compressedData = new byte[memStream.Length];
            memStream.Read(compressedData, 0, compressedData.Length);
            memResult = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, memResult, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, memResult, 0, 4);
            
            /*}
            catch
            {
                memResult = null;
            }*/
            return memResult;
        }



        /// <summary>
        /// Decompresses the buffer
        /// </summary>
        public static byte[] DecompressBuffer(ref byte[] buffer)
        {
            byte[] memResult = null;       
            /*try
            {*/
            MemoryStream memStream = new MemoryStream();
            int iLength = BitConverter.ToInt32(buffer, 0);
            memStream.Write(buffer, 4, buffer.Length - 4);
            memResult = new byte[iLength];
            memStream.Position = 0;
            GZipStream gZipStream = new GZipStream(memStream, CompressionMode.Decompress);
            gZipStream.Read(memResult, 0, memResult.Length);
            /*}
            catch
            {
                memResult = null;
            }*/
            return memResult;
        }

    }
}
