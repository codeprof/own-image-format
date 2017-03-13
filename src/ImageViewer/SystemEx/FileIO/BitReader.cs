//Date: 2009-11-14
//Author: smoebius
//Description: simple, but fast bit reader class
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//The author is not liable for any damage caused by this software.
//Licenced under MIT licence
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SystemEx.FileIO
{
    /// <summary>
    /// Provides methods to read single bits from a stream
    /// </summary>
    public class BitReader
    {
        //private int iPos = 0;
        //private bool bCache = false;
        //private int[] tmpBuffer = null;
        //private byte[] tmpByteBuffer = null;
        private Stream strm = null;
        private bool bEndOfStream;
        private bool bExceptionIfEndOfStream;
        private int iTmpValue = 0;
        private int iAlreadyReadBits = 0;

        private void InitBitReader(Stream stream, bool bUseCache)
        {
            if (stream == null)
            {
                throw new ArgumentException("stream cannot be null!");
            }
            strm = stream;

            /* At the moment there is no cache support...
            bCache = false;
            if (bUseCache) //Use cache only if possible
            {
                bCache = strm.CanSeek;
            }

            if (bCache == true)
            {
                //Do not initialize buffers if they are not needed
                tmpBuffer = new int[4096];
                tmpByteBuffer = new byte[4096 * 4];

                for (int i = 0; i < 4096; i++)
                {
                    tmpBuffer[i] = 0;
                }
                for (int i = 0; i < 4096; i++)
                {
                    tmpByteBuffer[i] = 0;
                }
            }
            */
            iAlreadyReadBits = 8;
            bEndOfStream = false;
            bExceptionIfEndOfStream = false;
        }

        /// <summary>
        /// Creates a BitReader object
        /// </summary>
        /// <param name="stream">stream object on which the BitReaser object is based</param>
        /// <exception cref="ArgumentException"></exception>
        public BitReader(Stream stream)
        {
            InitBitReader(stream, false);
        }
        /*
        public BitReader(Stream stream, bool bUseCahce)
        {
            InitBitReader(stream, bUseCahce);
        }
        */

        /*
        /// <summary>
        /// Returns whether the BitReader is cached
        /// </summary>
        public bool Cached
        {
            get
            {
                return bCache;
            }
        }
        */

        private void HandleEndOfStream()
        {
            bEndOfStream = true;
            if (bExceptionIfEndOfStream)
            {
                throw new System.IO.EndOfStreamException();
            }
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
        /// Reads a single bit form the stream
        /// </summary>
        /// <returns>boolean value (true or false)</returns>
        public bool ReadBit()
        {
            /*
            if (bCache)
            {
                return false;
            }
            else
            {
            */

            if (iAlreadyReadBits == 8)
            {
                iTmpValue = strm.ReadByte();
                if (iTmpValue == -1) { HandleEndOfStream(); }
                iAlreadyReadBits = 1;
                if ((iTmpValue & 1) == 1)
                {
                    return true;
                }
            }
            else
            {
                if ((iTmpValue & (1 << iAlreadyReadBits++)) != 0) // increases iAlreadyReadBits after the compare
                {
                    return true;
                }
            }
            return false;

            //}
        }

        /// <summary>
        /// Reads a single bit form the stream
        /// </summary>
        /// <returns>integer value (1 or 0)</returns>
        public int ReadBitAsInt()
        {
            if (iAlreadyReadBits == 8)
            {
                iTmpValue = strm.ReadByte();
                if (iTmpValue == -1) { HandleEndOfStream(); }
                iAlreadyReadBits = 1;
                return iTmpValue & 1;
            }
            else
            {
                return (iTmpValue & (1 << iAlreadyReadBits++)); // increases iAlreadyReadBits after the return
            }
        }

        /// <summary>
        /// Reads bits form the stream
        /// </summary>
        /// <returns>integer value</returns>
        public int ReadBits(int count)
        {
            int iRemainingBits = 8 - iAlreadyReadBits;
            int result = 0;

            if (count <= iRemainingBits)
            {
                //return (iTmpValue >> (iAlreadyReadBits += count)) & ((1 << count) - 1); // Is not correct!
                result = (iTmpValue >> iAlreadyReadBits) & ((1 << count) - 1);
                iAlreadyReadBits += count;
            }
            else
            {
                result = (iTmpValue >> iAlreadyReadBits) & ((1 << count) - 1);
                count -= iRemainingBits;
                //count must be still greater than 0 here...
                if (count <= 8)
                {
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += (iTmpValue & ((1 << count) - 1)) << iRemainingBits;
                    iAlreadyReadBits = count;
                }
                else if (count <= 16)
                {
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += iTmpValue << iRemainingBits;
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    count -= 8;
                    result += (iTmpValue & ((1 << count) - 1)) << (iRemainingBits + 8);
                    iAlreadyReadBits = count;
                }
                else if (count <= 24)
                {
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += iTmpValue << iRemainingBits;
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += iTmpValue << (iRemainingBits + 8);
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    count -= 16;
                    result += (iTmpValue & ((1 << count) - 1)) << (iRemainingBits + 16);
                    iAlreadyReadBits = count;
                }
                else if (count <= 32)
                {
                    //not simplified by a additonal method because of speed reason...
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += iTmpValue << iRemainingBits;
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += iTmpValue << (iRemainingBits + 8);
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    result += iTmpValue << (iRemainingBits + 16);
                    iTmpValue = strm.ReadByte();
                    if (iTmpValue == -1) { HandleEndOfStream(); }
                    count -= 24;
                    result += (iTmpValue & ((1 << count) - 1)) << (iRemainingBits + 24);
                    iAlreadyReadBits = count;
                }
            }
            return result;
        }

        /// <summary>
        /// returns the current byte position
        /// </summary>
        public long BytePosition
        {
            get
            {
                return (long)strm.Position;
            }
        }

        /// <summary>
        /// returns the current bit position
        /// </summary>
        public long BitPosition
        {
            get
            {
                return (long)(strm.Position << 3) - 8 + iAlreadyReadBits;
            }
        }

        /// <summary>
        /// Returns the underlaying stream object
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            return strm;
        }


        /// <summary>
        /// Closes the BitReader object
        /// </summary>
        /// <param name="closeStream">If true the underlaying stream will be also closed</param>
        public void Close(bool closeStream)
        {
            strm.Flush();
            if (closeStream)
            {
                strm.Close();
            }
            strm = null;
        }

        /// <summary>
        /// Closes the BitReader object but does not close the underlaying stream
        /// </summary>
        public void Close()
        {
            Close(false);
        }



    }
}
