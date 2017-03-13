//Date: 2009-09-20
//Author: smoebius
//Description: Provides methods to write to/read form streams.  
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SystemEx.FileIO
{
    /// <summary>
    /// Provides methods to write to/read form streams.  
    /// </summary>
    public class StreamHelper
    {

        #pragma warning disable 1591 // do not show compiler warnings of the missing descriptions
        public enum StringFormat : int
        {
            ASCII,
            UTF8
        }
        #pragma warning restore 1591

        private Stream oStream;
        private bool bEndOfStream;
        private bool bExceptionIfEndOfStream;
        private byte[] buffer_boolean_true;
        private byte[] buffer_boolean_false;
        private byte[] readBuffer;
        private bool bCache;

        /// <summary>
        /// Creates a new StreamHelper object for the declared stream. If stream is null, a MemoryStream will be created.
        /// </summary>
        /// <param name="stream">the underlaying Stream object</param>
        public StreamHelper(Stream stream)
        {
            if (stream != null)
            {
                oStream = stream;
            }
            else
            {
                oStream = new MemoryStream();
            }

            bCache = true;
            bExceptionIfEndOfStream = true;
            bEndOfStream = false;

            readBuffer = null;
            buffer_boolean_true = BitConverter.GetBytes(true);
            buffer_boolean_false = BitConverter.GetBytes(false);     
        }

        /// <summary>
        /// Creates a new file with read and write access. If the file already exists, it will be overwritten.
        /// </summary>
        /// <param name="file">path to the file</param>
        /// <returns></returns>
        public static StreamHelper CreateFile(string file)
        {
            Stream strm = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
            return new StreamHelper(strm);
        }

        /// <summary>
        /// Reads an already existing file (only read access). The file will not be created if it does not exists.
        /// </summary>
        /// <param name="file">path to the file</param>
        /// <returns></returns>
        public static StreamHelper ReadFile(string file)
        {
            Stream strm = new FileStream(file, FileMode.Open, FileAccess.Read);
            return new StreamHelper(strm);
        }

        /// <summary>
        /// Opens a new file with read and write access. If the file not not exists, it will be created.
        /// </summary>
        /// <param name="file">path to the file</param>
        /// <returns></returns>
        public static StreamHelper OpenFile(string file)
        {
            Stream strm = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return new StreamHelper(strm);
        }


        /* Does not work correctly
        /// <summary>
        /// Opens a new file with read and write access. If the file not not exists, it will be created.
        /// </summary>
        /// <param name="file">path to the file</param>
        /// <returns></returns>
        public static StreamHelper ReadHTTPFile(string file)
        {
            System.Net.WebRequest web = System.Net.HttpWebRequest.Create("http://localhost:8080/Wikipedia/A/Guy+Maunsell");
            return new StreamHelper(web.GetResponse().GetResponseStream());
        }
        */

        /// <summary>
        /// Enables or disables write cache (the default is true)
        /// </summary>
        public bool WriteCache
        {
            get
            {
                return bCache;
            }
            set
            {
                bCache = value;
            }
        }

        /// <summary>
        /// returns the underlaying Stream object
        /// </summary>
        public Stream GetStream()
        {
            return oStream;
        }

        /// <summary>
        /// returns whether the end of the stream has been reached
        /// </summary>
        public bool EndOfStream
        {
            get
            {
                return this.bEndOfStream;
            }
        }

        /// <summary>
        /// returns whether the stream can be read
        /// </summary>
        public bool CanRead
        {
            get
            {
                return this.oStream.CanRead;
            }
        }

        /// <summary>
        /// returns whether the stream can be written
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return this.oStream.CanWrite;
            }
        }

        /// <summary>
        /// returns whether the stream supports seeking
        /// </summary>
        public bool CanSeek
        {
            get
            {
                return this.oStream.CanSeek;
            }
        }


        /// <summary>
        /// returns whether the stream can time out
        /// </summary>
        public bool CanTimeout
        {
            get
            {
                return this.oStream.CanTimeout;
            }
        }

        /// <summary>
        /// defines whether an exception should be thrown if the end of the stream has been reached (default is false)
        /// </summary>
        public bool ThrowEndOfStreamException
        {
            get
            {
                return bExceptionIfEndOfStream;
            }
            set
            {
                bExceptionIfEndOfStream = value;
            }
        }

        /// <summary>
        /// gets or sets the current byte position of the stream
        /// </summary>
        public long Position
        {
            get
            {
                return oStream.Position;
            }
            set
            {
                oStream.Position = value;
            }
        }

        /// <summary>
        /// gets the current byte length of the stream
        /// </summary>
        public long Length
        {
            get
            {
                return oStream.Length;
            }
        }


        /// <summary>
        /// Writes a 64 bit signed integer value to the stream.
        /// </summary>
        /// <param name="lValue">64 bit signed integer</param>
        public void WriteLong(long lValue)
        {
            byte[] buffer = BitConverter.GetBytes(lValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 64 bit unsigned integer value to the stream.
        /// </summary>
        /// <param name="lValue">64 bit unsigned integer</param>
        public void WriteULong(ulong lValue)
        {
            byte[] buffer = BitConverter.GetBytes(lValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 32 bit signed integer value to the stream.
        /// </summary>
        /// <param name="iValue">32 bit signed integer</param>
        public void WriteInt(int iValue)
        {
            byte[] buffer = BitConverter.GetBytes(iValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 32 bit unsigned integer value to the stream.
        /// </summary>
        /// <param name="iValue">32 bit unsigned integer</param>
        public void WriteUInt(uint iValue)
        {
            byte[] buffer = BitConverter.GetBytes(iValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 16 bit signed integer value to the stream.
        /// </summary>
        /// <param name="iValue">16 bit signed integer</param>
        public void WriteShort(short iValue)
        {
            byte[] buffer = BitConverter.GetBytes(iValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 16 bit unsigned integer value to the stream.
        /// </summary>
        /// <param name="iValue">16 bit unsigned integer</param>
        public void WriteUShort(ushort iValue)
        {
            byte[] buffer = BitConverter.GetBytes(iValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 32 bit signed float point value to the stream. 
        /// </summary>
        /// <param name="fValue">signed 32 bit float value</param>
        public void WriteFloat(float fValue)
        {
            byte[] buffer = BitConverter.GetBytes(fValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a 64 bit signed double to the stream. 
        /// </summary>
        /// <param name="dValue">signed 64 bit double</param>
        public void WriteDouble(double dValue)
        {
            byte[] buffer = BitConverter.GetBytes(dValue);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a boolean to the stream.
        /// </summary>
        /// <param name="bValue">boolean value</param>
        public void WriteBool(bool bValue)
        {
            if (bValue)
            {
                oStream.Write(buffer_boolean_true, 0, sizeof(bool));
            }
            else
            {
                oStream.Write(buffer_boolean_false, 0, sizeof(bool));
            }
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <param name="bValue">byte value</param>
        public void WriteByte(byte bValue)
        {
            oStream.WriteByte(bValue);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a byte buffer to the stream.
        /// </summary>
        /// <param name="buffer">byte buffer</param>
        public void WriteData(byte[] buffer)
        {
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a byte buffer to the stream.
        /// </summary>
        /// <param name="buffer">byte buffer</param>
        /// <param name="start">index of the first byte</param>
        /// <param name="count">number of bytes to write</param>
        public void WriteDataEx(byte[] buffer, int start, int count)
        {
            oStream.Write(buffer, start, count);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a string to the stream.
        /// </summary>
        /// <param name="sText">string value</param>
        /// <param name="format">format of the string</param>
        public void WriteString(string sText, StringFormat format)
        {
            byte[] buffer;
            if (format == StringFormat.UTF8)
            {
                buffer = Encoding.UTF8.GetBytes(sText);
            }
            else
            {
                buffer = Encoding.ASCII.GetBytes(sText);
            }

            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a string to the stream (UTF8 format will be used).
        /// </summary>
        /// <param name="sText">string value</param>
        public void WriteString(string sText)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(sText);
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a string with end line character to the stream.
        /// </summary>
        /// <param name="sText">string value</param>
        /// <param name="format">format of the string</param>
        public void WriteStringLine(string sText, StringFormat format)
        {

            byte[] buffer;
            if (format == StringFormat.UTF8)
            {
                buffer = Encoding.UTF8.GetBytes(sText + "\n");
            }
            else
            {
                buffer = Encoding.ASCII.GetBytes(sText + "\n");
            }

            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }

        /// <summary>
        /// Writes a string with end line character to the stream (UTF8 format will be used).
        /// </summary>
        /// <param name="sText">string value</param>
        public void WriteStringLine(string sText)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(sText + "\n");
            oStream.Write(buffer, 0, buffer.Length);
            if (bCache == false)
            {
                oStream.Flush();
            }
        }


        /// <summary>
        /// Closes the stream
        /// </summary>
        public void Close()
        {
            if (readBuffer != null)
            {
                readBuffer = null;
            }
            oStream.Close();
            oStream = null;
        }

        /// <summary>
        /// Writes all cached data to the stream
        /// </summary>
        public void Flush()
        {
            oStream.Flush();
        }

        /// <summary>
        /// Reads data from the stream
        /// </summary>
        /// <param name="iSize">number of bytes to read</param>
        /// <returns>byte array</returns>
        public byte[] ReadData(int iSize)
        {
            // Now read into a byte buffer.
            byte[] bytes = new byte[iSize];
            int numBytesToRead = (int)iSize;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = oStream.Read(bytes, numBytesRead, numBytesToRead);

                if (n <= 0) // 0 means the end of the file is reached.
                {
                    break;
                }
                numBytesRead += n;
                numBytesToRead -= n;
            }
            if (numBytesToRead > 0)
            {
                bEndOfStream = true;
                if (bExceptionIfEndOfStream)
                {
                    throw new System.IO.EndOfStreamException();
                }
            }
            return bytes;
        }

        /// <summary>
        /// reads an 32 bit signed integer from the stream.
        /// </summary>
        /// <returns>32 bit signed integer value</returns>
        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadData(sizeof(int)), 0);
        }

        /// <summary>
        /// reads an 32 bit unsigned integer from the stream.
        /// </summary>
        /// <returns>32 bit unsigned integer value</returns>
        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(ReadData(sizeof(uint)), 0);
        }

        /// <summary>
        /// reads an 16 bit signed integer from the stream.
        /// </summary>
        /// <returns>16 bit signed integer value</returns>
        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadData(sizeof(Int16)), 0);
        }

        /// <summary>
        /// reads an 16 bit unsigned integer from the stream.
        /// </summary>
        /// <returns>16 bit unsigned integer value</returns>
        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadData(sizeof(UInt16)), 0);
        }

        /// <summary>
        /// reads an 32 bit float from the stream.
        /// </summary>
        /// <returns>32 bit float value</returns>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadData(sizeof(float)), 0);
        }

        /// <summary>
        /// reads an 64 bit double from the stream.
        /// </summary>
        /// <returns>64 bit double value</returns>
        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadData(sizeof(double)), 0);
        }

        /// <summary>
        /// reads a boolean from the stream.
        /// </summary>
        /// <returns>boolean value</returns>
        public bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadData(sizeof(bool)), 0);
        }

        /// <summary>
        /// reads a byte from the stream.
        /// </summary>
        /// <returns>byte</returns>
        public byte ReadByte()
        {
            int iValue = oStream.ReadByte();
            if (iValue < 0)
            {
                bEndOfStream = true;
                if (bExceptionIfEndOfStream)
                {
                    throw new System.IO.EndOfStreamException();
                }
            }
            return (byte)iValue;
        }

        /// <summary>
        /// reads a string from the stream.
        /// </summary>
        /// <param name="format">The format of the string</param>
        /// <returns>string</returns>
        public string ReadString(StringFormat format)
        {
            if (readBuffer == null)
            {
                readBuffer = new byte[32768];
            }
            byte[] tmpBuffer = readBuffer;
            byte[] newBuffer = null; 
            string result = "";
            int iValue = oStream.ReadByte();
            int iPos = 0;
            int iMaxSize = 32768;
            while (iValue != -1 && iValue != 0 && iValue != '\n')
            {
                tmpBuffer[iPos] += (byte)iValue;
                iPos++;
                if (iPos >= iMaxSize - 1)
                {
                   iMaxSize = iPos * 2;
                   newBuffer = new byte[iMaxSize];
                   tmpBuffer.CopyTo(newBuffer, 0);
                   tmpBuffer = newBuffer;
                }
                iValue = oStream.ReadByte();
            }
            if (iValue < 0)
            {
                bEndOfStream = true;
                if (bExceptionIfEndOfStream)
                {
                    throw new System.IO.EndOfStreamException();
                }
            }
            if (format == StringFormat.UTF8)
            {
                result = Encoding.UTF8.GetString(tmpBuffer, 0, iPos);
            }
            else
            {
                result = Encoding.ASCII.GetString(tmpBuffer, 0, iPos);
            }
            return result;
        }

        /// <summary>
        /// reads a string in UTF8 format from the stream.
        /// </summary>
        /// <returns>string</returns>
        public string ReadString()
        {
            return ReadString(StringFormat.UTF8);
        }

    }
}
