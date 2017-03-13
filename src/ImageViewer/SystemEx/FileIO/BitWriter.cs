//Date: 2009-11-01
//Author: smoebius
//Description: simple, but very fast bit writer class
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SystemEx.FileIO
{
    /// <summary>
    /// Fast bit writer class.
    /// </summary>
    public class BitWriter
    {
        //It should be faster than System.Collections.BitArray!
        private int iPos = 0;
        private int[] tmpBuffer = null;
        private byte[] tmpByteBuffer = null;
        private Stream strm = null;
        private const int CACHE_SIZE = 1024;

        /// <summary>
        /// creates a BitWriter object
        /// </summary>
        /// <param name="stream">stream object on which the BitWriter object is based</param>
        /// <exception cref="ArgumentException"></exception>
        public BitWriter(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentException("stream cannot be null!");
            }

            tmpBuffer = new int[CACHE_SIZE];
            tmpByteBuffer = new byte[CACHE_SIZE * 4];

            for (int i = 0; i < CACHE_SIZE; i++)
            {
                tmpBuffer[i] = 0;
            }
            for (int i = 0; i < CACHE_SIZE * 4; i++)
            {
                tmpByteBuffer[i] = 0;
            }
            strm = stream;
        }

        /// <summary>
        /// Length of the stream in bytes
        /// </summary>
        public long ByteLength
        {
            get
            {
                long lLenght = strm.Length + (iPos >> 3); // already written bytes + bytes in the cache
                if ((iPos & 7) != 0) //If e.g. 9 bits are used, it should return 2!
                {
                    lLenght++;
                }
                return lLenght;
            }
        }

        /// <summary>
        /// Length of the stream in bits
        /// </summary>
        public long BitLength
        {
            get
            {
                return (strm.Length << 3) + iPos;
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
        /// Writes a single bit to the stream
        /// </summary>
        /// <param name="value">boolean value (true or false)</param>
        public void WriteBit(bool value) 
        {
            //int iPosition = iPos >> 5; // bit -> int32 
            if (iPos >= (CACHE_SIZE - 4) * 32)
            {
                Flush();
                //iPosition = 0;
            }
            if (value)
            {
                //tmpBuffer[iPosition] = tmpBuffer[iPosition] | (1 << (iPos & 31));   //% 32)); 
                tmpBuffer[iPos >> 5] += (1 << (iPos & 31));   //% 32)); 
            }
            /*
            else
            {
                tmpBuffer[iPos >> 5] = tmpBuffer[iPos >> 5] & (~(1 << (iPos % 32))); 
            }
            */
            iPos++;
        }

        /// <summary>
        /// Writes a single bit to the stream
        /// </summary>
        /// <param name="value">integer value (1 or 0)</param>
        public void WriteBit(int value)
        {
            if (iPos >= (CACHE_SIZE - 4) * 32)
            {
                Flush();
            }
            if ((value & 1) == 1)
            {
                tmpBuffer[iPos >> 5] += (1 << (iPos & 31));   //% 32)); 
            }
            iPos++;
        }

        /// <summary>
        /// Writes bits to the stream
        /// </summary>
        /// <param name="bits">integer value containing the bits</param>
        /// <param name="num">number of bts to write</param>
        public void WriteBits(int bits, int num)
        {
            if (iPos >= (1024 - 8) * 32)
            {
                Flush();
            }

            if (bits != 0)
            {
                if (num <= (32 - (iPos & 31))) // check if there are enought remaining bits in this 32bit block
                {
                    //all bits fit in the same 32bit block
                    tmpBuffer[iPos >> 5] += (bits << (iPos & 31));
                }
                else
                {
                    int iAvailableBits = 32 - (iPos & 31);
                    int iPosition = iPos >> 5;
                    tmpBuffer[iPosition] += ((bits & ((1 << iAvailableBits) - 1)) << (iPos & 31));
                    tmpBuffer[iPosition + 1] = (int)(((uint)bits) >> iAvailableBits); // it must be uint, because otherwise higher bits would be filled with 1!
                }
            }
            iPos += num;
        }

        /// <summary>
        /// Writes the cached bits to the underlaying stream. 
        /// </summary>
        public void Flush()
        {
            int iBytes = iPos >> 3; // bit -> byte
            int iInts = iPos >> 5;  // bit -> int32 
            Buffer.BlockCopy(tmpBuffer, 0, tmpByteBuffer, 0, iBytes);
            //tmpBuffer[0] = tmpBuffer[iInts];
            Buffer.BlockCopy(tmpBuffer, iBytes, tmpBuffer, 0, 8);
            for (int p = 1; p < iInts; p++)
            {
                tmpBuffer[p] = 0;
            }
            strm.Write(tmpByteBuffer, 0, iBytes);
            strm.Flush();
            iPos -= (iBytes << 3);
        }


        /// <summary>
        /// Closes the BitWriter object
        /// </summary>
        /// <param name="closeStream">If true the underlaying stream will be also closed</param>
        public void Close(bool closeStream)
        {
            Flush();
            if (iPos > 0)
            {
                strm.WriteByte((byte)(tmpBuffer[0] & 255));
            }
            strm.Flush();
            if (closeStream)
            {
                strm.Close();
            } 
            strm = null;
            tmpBuffer = null;
            tmpByteBuffer = null;
        }

        /// <summary>
        /// Closes the BitWriter object but does not close the underlaying stream
        /// </summary>
        public void Close()
        {
            Close(false);
        }


    }
}
