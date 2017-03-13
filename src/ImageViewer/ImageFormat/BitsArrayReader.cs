//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//The author is not liable for any damage caused by this software.
//Licenced under MIT licence
using System;
using System.Collections.Generic;
using System.Text;
using SystemEx.FileIO;
using System.IO;
using SystemEx.MathEx;

namespace SystemEx.ImageFormat2
{
    public class BitsArrayReader
    {
        BitReader br;
        public BitsArrayReader(Stream strm)
        {
            br = new BitReader(strm);
            br.ThrowEndOfStreamException = true;
        }

        private int ReadUInt30Bits()
        {
            int iValue = br.ReadBits(3);
            if (iValue == 7)
            {
                iValue += br.ReadBits(8);
                if (iValue == 7 + 255)
                {
                    iValue += br.ReadBits(30);
                }
            }
            return iValue;
        }

        public void Read(ref int[] array, int offset, int size, int min, int max)
        {
            throw new NotImplementedException();
        }

        public void Read(ref int[,] array, int offsetX, int offsetY, int width, int height, int min, int max)
        {
            int lastX = offsetX + width, lastY = offsetY + height;
            int count = max - min;
            int bits = IntegerMath.ILog2(IntegerMath.ToPowerOf2(Math.Abs(count)));

            if (max >= 0)
            {
                for (int y = offsetY; y < lastY; y++)
                {
                    for (int x = offsetX; x < lastX; x++)
                    {
                        array[x, y] = br.ReadBits(bits) + min;
                    }
                }
            }
            else
            {
                for (int y = offsetY; y < lastY; y++)
                {
                    for (int x = offsetX; x < lastX; x++)
                    {
                        array[x, y] = -br.ReadBits(bits) + min;
                    }
                }
            }

        }

    }
}
