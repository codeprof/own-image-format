//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Collections.Generic;
using System.Text;
using SystemEx.FileIO;
using System.IO;
using SystemEx.MathEx;

namespace SystemEx.ImageFormat2
{
    public class BitsArrayWriter
    {
        private BitWriter bw;

        public BitsArrayWriter(Stream strm)
        {
            bw = new BitWriter(strm);
        }

        private void WriteUInt30Bits(int n)
        {
            if ( n >= 7)
            {
                bw.WriteBits(7, 3);
                if (n >= 255 + 7)
                {
                    bw.WriteBits(255, 8);
                    bw.WriteBits(n - 255 - 7, 30);
                }
                else
                {
                    bw.WriteBits(n - 7, 8);
                }
            }
            else
            {
                bw.WriteBits(n, 3);
            }
        }

        public void Write(ref int[] array, int offset, int size, ref int min, ref int max)
        {
            throw new NotImplementedException();
        }

        public void Write(ref int[,] array, int offsetX, int offsetY, int width, int height, ref int posMin, ref int posMax, ref int negMin, ref int negMax)
        {
            int countPos, countNeg, bitsPos, bitsNeg, lastX = offsetX + width, lastY = offsetY + height;
            int val, iCount = 0, x, y;
            bool bDo, bLastZero;
            posMin = int.MaxValue;
            posMax = int.MinValue;
            negMin = int.MaxValue;
            negMax = int.MinValue;

            for (y = offsetY; y < lastY; y++)
            {
                for (x = offsetX; x < lastX; x++)
                {
                    val = array[x, y];
                    if (val > 0)
                    {
                        if (val > posMax)
                        {
                            posMax = val;
                        }
                        if (val < posMin)
                        {
                            posMin = val;
                        }
                    }
                    else if (val < 0)
                    {
                        if (val > negMax)
                        {
                            negMax = val;
                        }
                        if (val < negMin)
                        {
                            negMin = val;
                        }
                    }
                }
            }

            countPos = posMax - posMin;
            bitsPos = IntegerMath.ILog2(IntegerMath.ToPowerOf2(Math.Abs(countPos) + 1));
            countNeg = negMax - negMin;
            bitsNeg = IntegerMath.ILog2(IntegerMath.ToPowerOf2(Math.Abs(countNeg) + 1));
            x = 0;
            y = 0;
            bLastZero = false;
            if (0 >= 0)
            {
                val = array[x, y];

                bDo = true;
                while (bDo)
                {
                    iCount++;
                    val = array[x, y];
                    if ((val < 0) || (val > 0))
                    {
                        if (bLastZero == true)
                        {
                            bLastZero = false;
                            bDo = false;
                        }
                    }
                    else
                    {
                        if (bLastZero == false)
                        {
                            bLastZero = true;
                            bDo = false;
                        }
                    }
                    x++;
                    iCount++;
                    if (x > lastX)
                    {
                        y++;
                        x = offsetX;
                    }
                    if (y > lastY)
                    {
                        bDo = false;
                    }
                }         
   


                for (y = offsetY; y < lastY; y++)
                {
                    for (x = offsetX; x < lastX; x++)
                    {
                        val = array[x, y];

                        if (val > 0)
                        {
                            bw.WriteBits(val - posMin, bitsPos);
                        }
                        else if (val < 0)
                        {
                            bw.WriteBits(-val + negMax, bitsNeg);
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
            }
        }

    }
}
