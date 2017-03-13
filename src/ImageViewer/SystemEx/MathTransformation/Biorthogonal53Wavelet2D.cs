//Author: smoebius
//Date: 2009-12-22
//Description: impementation of a two dimensional biorthogonal 5/3 wavelet transformation
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;

namespace SystemEx.MathTransform
{
    /// <summary>
    /// impementation of a two dimensional biorthogonal 5/3 wavelet transformation
    /// </summary>
    public class Biorthogonal53Wavelet2D : WaveletPacket2D
    {
        /// <summary>
        /// Initalizes a two dimensional biorthogonal 5/3 wavelet transformation.
        /// </summary>
        /// <param name="PowerOf2Width">width of the wavelet transformation</param>
        /// <param name="PowerOf2Height">height of the wavelet transformation</param>
        public Biorthogonal53Wavelet2D(int PowerOf2Width, int PowerOf2Height)
            : base(2, 2, PowerOf2Width, PowerOf2Height, false)
        {
        }

        /// <summary>
        /// Initalizes a two dimensional biorthogonal 5/3 wavelet transformation.
        /// </summary>
        /// <param name="PowerOf2Width">width of the wavelet transformation</param>
        /// <param name="PowerOf2Height">height of the wavelet transformation</param>
        /// <param name="MinSize">minimum transformation size</param>
        public Biorthogonal53Wavelet2D(int PowerOf2Width, int PowerOf2Height, int MinSize)
            : base(MinSize, 2, PowerOf2Width, PowerOf2Height, false)
        {
        }

        /// <summary>
        /// Initalizes a two dimensional biorthogonal 5/3 wavelet transformation.
        /// </summary>
        /// <param name="PowerOf2Width">width of the wavelet transformation</param>
        /// <param name="PowerOf2Height">height of the wavelet transformation</param>
        /// <param name="MinSize">minimum transformation size</param>
        /// <param name="Scale">Use a Scale factor(default is false). This is particularly useful for image compressing.</param>
        /// <exception cref="ArgumentException"></exception>
        public Biorthogonal53Wavelet2D(int PowerOf2Width, int PowerOf2Height, int MinSize, bool Scale)
            : base(MinSize, 2, PowerOf2Width, PowerOf2Height, Scale)
        {
        }

        /// <summary>
        /// Initalizes a two dimensional biorthogonal 5/3 wavelet transformation.
        /// </summary>
        /// <param name="PowerOf2Width">width of the wavelet transformation</param>
        /// <param name="PowerOf2Height">height of the wavelet transformation</param>
        /// <param name="HorizontalMinSize">minimum horizontal transformation size</param>
        /// <param name="VerticalMinSize">minimum vertical transformation size</param>
        /// <param name="Scale">Use a Scale factor(default is false). This is particularly useful for image compressing.</param>
        /// <exception cref="ArgumentException"></exception>
        public Biorthogonal53Wavelet2D(int PowerOf2Width, int PowerOf2Height, int HorizontalMinSize, int VerticalMinSize, bool Scale)
            : base(HorizontalMinSize, VerticalMinSize, 2, PowerOf2Width, PowerOf2Height, Scale)
        {
        }

        #pragma warning disable 1591 // do not show compiler warnings of the missing descriptions
        override protected void TransformRow(ref float[,] array, int row, int start, int count)
        {
            if (count >= 2)
            {
                int half = count >> 1;
                int end = start + count;
                int j;
                int i = 0;
                float a, b, c;

                for (j = start; j < end - 3; j += 2)
                {
                    a = array[j, row];
                    b = array[j + 1, row];
                    c = array[j + 2, row];

                    tmp[i + half] = b - (a + c) * 0.5f;
                    tmp[i] = a;
                    i++;
                }

                //for the last two values a special transformation is needed (a, b-a)
                tmp[i] = array[end - 2, row];
                tmp[i + half] = array[end - 1, row] - array[end - 2, row];

                i = 0;
                for (j = start; j < end - 3; j += 2)
                {
                    // Apply approximation
                    b = tmp[i + half] * 0.25f;
                    tmp[i] += b;
                    tmp[i + 1] += b;
                    i++;
                }

                // Apply scale factors
                i = 0;
                if (bScale)
                {
                    for (j = start; j < half; j++)
                    {
                        array[j, row] = tmp[i] * K;
                        array[j + half, row] = tmp[i + half] * IK;
                        i++;
                    }
                }
                else
                {
                    for (j = start; j < end; j+=2)
                    {
                        array[j, row] = tmp[i];
                        array[j + 1, row] = tmp[i + 1];
                        i += 2;
                    }
                }
            }
        }

        override protected void TransformColumn(ref float[,] array, int column, int start, int count)
        {
            if (count >= 2)
            {
                int half = count >> 1;
                int end = start + count;
                int j;
                int i = 0;
                float a, b, c;

                for (j = start; j < end - 3; j += 2)
                {
                    a = array[column, j];
                    b = array[column, j + 1];
                    c = array[column, j + 2];

                    tmp[i + half] = b - (a + c) * 0.5f;
                    tmp[i] = a;
                    i++;
                }

                //for the last two values a special transformation is needed (a, b-a)
                tmp[i] = array[column, end - 2];
                tmp[i + half] = array[column, end - 1] - array[column, end - 2];

                i = 0;
                for (j = start; j < end - 3; j += 2)
                {
                    // Apply approximation
                    b = tmp[i + half] * 0.25f;
                    tmp[i] += b;
                    tmp[i + 1] += b;
                    i++;
                }

                // Apply scale factors
                i = 0;
                if (bScale)
                {
                    for (j = start; j < half; j++)
                    {
                        array[column, j] = tmp[i] * K;
                        array[column, j + half] = tmp[i + half] * IK;
                        i++;
                    }
                }
                else
                {
                    for (j = start; j < end; j += 2)
                    {
                        array[column, j] = tmp[i];
                        array[column, j + 1] = tmp[i + 1];
                        i += 2;
                    }
                }
            }
        }

        override protected void BackTransformRow(ref float[,] array, int row, int start, int count)
        {
            if (count >= 2)
            {
                int half = count >> 1;
                int endhalf = start + half;
                int end = start + count;
                int j = 0;
                int i = 0;
                float a, b, c;

                // remove scale factors
                if (bScale)
                {
                    for (j = start; j < end - 1; j += 2)
                    {
                        array[i, row] *= IK;
                        array[i + half, row] *= K;
                        i++;
                    }
                }

                // remove approximation
                for (i = start; i < endhalf - 1; i++)
                {
                    b = array[i + half, row] * 0.25f;
                    array[i, row] -= b;
                    array[i + 1, row] -= b;
                }

                j = 0;
                for (i = start; i < endhalf - 1; i++)
                {
                    a = array[i, row];
                    b = array[i + half, row];
                    c = array[i + 1, row];

                    tmp[j] = a;
                    tmp[j + 1] = b + (a + c) * 0.5f;

                    j += 2;
                }

                tmp[j] = array[i, row];
                tmp[j + 1] = array[i + half, row] + array[i, row];

                j = 0;
                for (i = start; i < end; i++)
                {
                    array[i, row] = tmp[j];
                    j++;
                }
            }
        }

        override protected void BackTransformColumn(ref float[,] array, int column, int start, int count)
        {
            if (count >= 2)
            {
                int half = count >> 1;
                int endhalf = start + half;
                int end = start + count;
                int j = 0;
                int i = 0;
                float a, b, c;

                // remove scale factors
                if (bScale)
                {
                    for (j = start; j < end - 1; j += 2)
                    {
                        array[column, i] *= IK;
                        array[column, i + half] *= K;
                        i++;
                    }
                }

                // remove approximation
                for (i = start; i < endhalf - 1; i++)
                {
                    b = array[column, i + half] * 0.25f;
                    array[column, i] -= b;
                    array[column, i + 1] -= b;
                }

                j = 0;
                for (i = start; i < endhalf - 1; i++)
                {
                    a = array[column, i];
                    b = array[column, i + half];
                    c = array[column, i + 1];

                    tmp[j] = a;
                    tmp[j + 1] = b + (a + c) * 0.5f;

                    j += 2;
                }

                tmp[j] = array[column, i];
                tmp[j + 1] = array[column, i + half] + array[column, i];

                j = 0;
                for (i = start; i < end; i++)
                {
                    array[column, i] = tmp[j];
                    j++;
                }
            }
        }
        #pragma warning restore 1591

    }
}
