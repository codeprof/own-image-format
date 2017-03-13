//Date: 2009-11-14
//Author: smoebius
//Date: 2009-12-12
//Description: helper functions to manipulate 2d arrays
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Collections.Generic;

namespace SystemEx
{
    /// <summary>
    /// helper functions to manipulate 2d arrays, especially for imaging purpose
    /// </summary>
    public static class ArrayHelper2D
    {
        #pragma warning disable 1591 // do not show compiler warnings of the missing descriptions
        public enum ResizeFilter : int
        {
            NONE_ZERO,
            NONE_CLAMP,
            POINT,
            LINEAR
        }
        #pragma warning restore 1591

        /// <summary>
        /// Calculates the Peak signal-to-noise ratio(PSNR) for two 2d double arrays.
        /// When the two arrays are identical the MSE will be zero which will result in an infinite PSNR.
        /// Typical values of the PSNR for lossy image compression are between 30 and 50 dB, where higher is better.
        /// </summary>
        /// <param name="array1">2d array with the content of a image</param>
        /// <param name="array2">2d array with the content of a image</param>
        /// <param name="width">with of the two images</param>
        /// <param name="height">height of the two images</param>
        /// <param name="MaxAmplitude">The maximum possible value. When the array contains data with 8 bit(0-255), this would be 255</param>
        /// <returns>Peak signal-to-noise ratio(PSNR)</returns>
        /// <exception cref="ArgumentException"></exception>       
        public static double Calc2DPSNR(ref double[,] array1, ref double[,] array2, int width, int height, double MaxAmplitude)
        {
            double MSE = 0.0d;
            double PSNR;
            double difference;
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width and height must be greater than zero");
            }
            if (array1 == null)
            {
                throw new ArgumentException("paramaeter array1 cannot be null");
            }
            if (array2 == null)
            {
                throw new ArgumentException("paramaeter array2 cannot be null");
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    difference = array2[x, y] - array1[x, y];
                    MSE += difference * difference;
                }
            }
            MSE /= (double)(width * height);

            PSNR = 20.0d * Math.Log10(MaxAmplitude / Math.Sqrt(MSE));
            return PSNR;
        }

        /// <summary>
        /// Calculates the peak signal-to-noise ratio(PSNR) for two 2d float arrays.
        /// When the two arrays are identical the MSE will be zero which will result in an infinite PSNR.
        /// Typical values of the PSNR for lossy image compression are between 30 and 50 dB, where higher is better.
        /// </summary>
        /// <param name="array1">2d array with the content of a image</param>
        /// <param name="array2">2d array with the content of a image</param>
        /// <param name="width">with of the two images</param>
        /// <param name="height">height of the two images</param>
        /// <param name="MaxAmplitude">The maximum possible value. When the array contains data with 8 bit(0-255), this would be 255</param>
        /// <returns>Peak signal-to-noise ratio(PSNR)</returns>
        /// <exception cref="ArgumentException"></exception>
        public static float Calc2DPSNR(ref float[,] array1, ref float[,] array2, int width, int height, float MaxAmplitude)
        {
            float MSE = 0.0f;
            float PSNR;
            float difference;
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width and height must be greater than zero");
            }
            if (array1 == null)
            {
                throw new ArgumentException("paramaeter array1 cannot be null");
            }
            if (array2 == null)
            {
                throw new ArgumentException("paramaeter array2 cannot be null");
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    difference = array2[x, y] - array1[x, y];
                    MSE += difference * difference;
                }
            }
            MSE /= (float)(width * height);

            PSNR = 20.0f * (float)Math.Log10(MaxAmplitude / Math.Sqrt(MSE));
            return PSNR;
        }

        /// <summary>
        /// Calculates the Peak signal-to-noise ratio(PSNR) for two 2d integer arrays.
        /// When the two arrays are identical the MSE will be zero which will result in an infinite PSNR.
        /// Typical values of the PSNR for lossy image compression are between 30 and 50 dB, where higher is better.
        /// </summary>
        /// <param name="array1">2d array with the content of a image</param>
        /// <param name="array2">2d array with the content of a image</param>
        /// <param name="width">with of the two images</param>
        /// <param name="height">height of the two images</param>
        /// <param name="MaxAmplitude">The maximum possible value. When the array contains data with 8 bit(0-255), this would be 255</param>
        /// <returns>Peak signal-to-noise ratio(PSNR)</returns>
        /// <exception cref="ArgumentException"></exception>
        public static double Calc2DPSNR(ref int[,] array1, ref int[,] array2, int width, int height, int MaxAmplitude)
        {
            int MSE = 0;
            double PSNR;
            int difference;
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width and height must be greater than zero");
            }
            if (array1 == null)
            {
                throw new ArgumentException("paramaeter array1 cannot be null");
            }
            if (array2 == null)
            {
                throw new ArgumentException("paramaeter array2 cannot be null");
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    difference = array2[x, y] - array1[x, y];
                    MSE += difference * difference;
                }
            }
            MSE /= (int)(width * height);

            PSNR = 20.0d * Math.Log10(((double)MaxAmplitude) / Math.Sqrt((double)MSE));
            return PSNR;
        }

        /// <summary>
        /// Sets the smallest coefficients of the 2d array to zero. Note: the resulting number of remaining coefficients may be not exactly numRemainingCoefficients.
        /// </summary>
        /// <param name="array">array to process</param>
        /// <param name="width">width of the array</param>
        /// <param name="height">height of the array</param>
        /// <param name="numRemainingCoefficients">number of remaining coefficients</param>
        /// <exception cref="ArgumentException"></exception>
        public static void RemoveSmallestCoefficients2D(ref double[,] array, int width, int height, int numRemainingCoefficients)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width and height must be greater than zero");
            }
            if (array == null)
            {
                throw new ArgumentException("paramaeter array cannot be null");
            }

            if (numRemainingCoefficients < width * height)
            {
                List<double> tmp = new List<double>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tmp.Add(Math.Abs(array[x, y]));
                    }
                }
                tmp.Sort();

                double minValue = 0.0d;

                minValue = tmp[(width * height - 1) - numRemainingCoefficients];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        if (Math.Abs(array[x, y]) < minValue)
                        {
                            array[x, y] = 0.0d;
                        }
                    }
                }
            }
            else
            {
                //not necessary to remove any coefficient...
            }
        }

        /// <summary>
        /// Sets the smallest coefficients of the 2d array to zero. Note: the resulting number of remaining coefficients may be not exactly numRemainingCoefficients.
        /// </summary>
        /// <param name="array">array to process</param>
        /// <param name="width">width of the array</param>
        /// <param name="height">height of the array</param>
        /// <param name="numRemainingCoefficients">number of remaining coefficients</param>
        /// <exception cref="ArgumentException"></exception>
        public static void RemoveSmallestCoefficients2D(ref float[,] array, int width, int height, int numRemainingCoefficients)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width and height must be greater than zero");
            }
            if (array == null)
            {
                throw new ArgumentException("paramaeter array cannot be null");
            }

            if (numRemainingCoefficients < width * height)
            {
                List<float> tmp = new List<float>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tmp.Add(Math.Abs(array[x, y]));
                    }
                }
                tmp.Sort();

                float minValue = 0.0f;

                minValue = tmp[(width * height - 1) - numRemainingCoefficients];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (Math.Abs(array[x, y]) < minValue)
                        {
                            array[x, y] = 0.0f;
                        }
                    }
                }
            }
            else
            {
                //not necessary to remove any coefficient...
            }
        }

        /// <summary>
        /// Sets the smallest coefficients of the 2d array to zero. Note: the resulting number of remaining coefficients may be not exactly numRemainingCoefficients.
        /// </summary>
        /// <param name="array">array to process</param>
        /// <param name="width">width of the array</param>
        /// <param name="height">height of the array</param>
        /// <param name="numRemainingCoefficients">number of remaining coefficients</param>
        /// <exception cref="ArgumentException"></exception>
        public static void RemoveSmallestCoefficients2D(ref int[,] array, int width, int height, int numRemainingCoefficients)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("width and height must be greater than zero");
            }
            if (array == null)
            {
                throw new ArgumentException("paramaeter array cannot be null");
            }

            if (numRemainingCoefficients < width * height)
            {
                List<int> tmp = new List<int>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tmp.Add(Math.Abs(array[x, y]));
                    }
                }
                tmp.Sort();

                int minValue = 0;

                minValue = tmp[(width * height - 1) - numRemainingCoefficients];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        if (Math.Abs(array[x, y]) < minValue)
                        {
                            array[x, y] = 0;
                        }
                    }
                }
            }
            else
            {
                //not necessary to remove any coefficient...
            }
        }

        /// <summary>
        /// Sharpens the values of the 2 dimensional array
        /// </summary>
        /// <param name="src">source array</param>
        /// <param name="dst">destination array</param>
        /// <param name="width">width of source and destionation array</param>
        /// <param name="height">height of source and destionation array</param>
        /// <param name="factor">strength of the sharpen effect</param>
        public static void SharpenArray2D(ref float[,] src, ref float[,] dst, int width, int height, float factor)
        {
            float a, b, c, d, tmp1, tmp2;
            factor = Math.Abs(factor);
            for (int y = 0; y < height-1; y++)
            {
                for (int x=0; x < width-1; x++)
                {
                    a = src[x, y];
                    b = src[x + 1, y];
                    c = src[x, y + 1];
                    d = src[x + 1, y + 1];

                    tmp1 =  a + (a - b) * factor;
                    tmp2 =  c + (c - d) * factor;
                    dst[x, y] = tmp1 + (tmp1 - tmp2) * factor;
                }
            }
        }

        /// <summary>
        /// Sharpens the values of the 2 dimensional array
        /// </summary>
        /// <param name="src">source array</param>
        /// <param name="dst">destination array</param>
        /// <param name="width">width of source and destionation array</param>
        /// <param name="height">height of source and destionation array</param>
        /// <param name="factor">strength of the sharpen effect</param>
        public static void SharpenArray2D(ref double[,] src, ref double[,] dst, int width, int height, double factor)
        {
            double a, b, c, d, tmp1, tmp2;
            factor = Math.Abs(factor);
            for (int y = 0; y < height-1; y++)
            {
                for (int x=0; x < width-1; x++)
                {
                    a = src[x, y];
                    b = src[x + 1, y];
                    c = src[x, y + 1];
                    d = src[x + 1, y + 1];

                    tmp1 =  a + (a - b) * factor;
                    tmp2 =  c + (c - d) * factor;
                    dst[x, y] = tmp1 + (tmp1 - tmp2) * factor;
                }
            }
        }

        /// <summary>
        /// Resizes the src array with the declared filter and stores the result in the dst array. 
        /// </summary>
        /// <param name="src">source array</param>
        /// <param name="dst">destination array</param>
        /// <param name="width">width of source array</param>
        /// <param name="height">height of source array</param>
        /// <param name="newWidth">width of destination array</param>
        /// <param name="newHeight">height of destination array</param>
        /// <param name="filter">filter which should be used to resize the array</param>
        public static void ResizeArray2D(ref double[,] src, ref double[,] dst, int width, int height, int newWidth, int newHeight, ResizeFilter filter)
        {
            int minWidth = Math.Min(width, newWidth);
            int minHeight = Math.Min(height, newHeight);
            double tmp;

            if (filter == ResizeFilter.NONE_ZERO)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    for (int x = 0; x < minWidth; x++)
                    {
                        dst[x, y] = src[x, y];
                    }
                }
                for (int y = 0; y < minHeight; y++)
                {
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = 0.0d;
                    }
                }
                for (int y = minHeight; y < newHeight; y++)
                {
                    for (int x = 0; x < minWidth; x++)
                    {
                        dst[x, y] = 0.0d;
                    }
                }
                for (int y = minHeight; y < newHeight; y++)
                {
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = 0.0d;
                    }
                }
            }

            if (filter == ResizeFilter.NONE_CLAMP)
            {

                for (int y = 0; y < minHeight; y++)
                {
                    for (int x = 0; x < minWidth; x++)
                    {
                        dst[x, y] = src[x, y];
                    }
                }
                for (int y = 0; y < minHeight; y++)
                {
                    tmp = src[minWidth - 1, y];
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = tmp;
                    }
                }

                for (int x = 0; x < minWidth; x++)
                {
                    tmp = src[x, minHeight - 1];
                    for (int y = minHeight; y < newHeight; y++)
                    {

                        dst[x, y] = tmp;
                    }
                }

                tmp = src[minWidth - 1, minHeight - 1];
                for (int y = minHeight; y < newHeight; y++)
                {
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = tmp;
                    }
                }
            }

            if (filter == ResizeFilter.POINT)
            {
                int iWidth2 = width - 1, iHeight2 = height - 1;
                int iNewWidth2 = newWidth - 1, iNewHeight2 = newHeight - 1;

                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        dst[x, y] = src[(x * iWidth2) / iNewWidth2, (y * iHeight2) / iNewHeight2];
                    }
                }
            }

            
            if (filter == ResizeFilter.LINEAR) // Does not work really good for downscaling
            {
                double dRestX, dRestY;
                double dPosX, dPosY;
                int iPosX, iPosY, iPosX2, iPosY2;
                int iWidth2 = width - 1, iHeight2 = height - 1;

                double dWidth2 = width - 1, dHeight2 = height - 1;
                double dNewWidth2 = newWidth - 1, dNewHeight2 = newHeight - 1;


                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        dPosX = (x * dWidth2) / dNewWidth2;
                        dPosY = (y * dHeight2) / dNewHeight2;
                        //dRestX = dPosX % 1.0d;
                        //dRestY = dPosY % 1.0d;
                        iPosX = (int)dPosX;
                        iPosY = (int)dPosY;
                        dRestX = dPosX - iPosX;
                        dRestY = dPosY - iPosY;

                        if ((iPosX < iWidth2) && (iPosY < iHeight2))
                        {
                            dst[x, y] = (1.0d - dRestY) * ((1.0d - dRestX) * src[iPosX, iPosY] + dRestX * src[iPosX + 1, iPosY]) + dRestY * ((1.0d - dRestX) * src[iPosX, iPosY + 1] + dRestX * src[iPosX + 1, iPosY + 1]);
                        }
                        else
                        {
                            iPosX2 = iPosX + 1;
                            iPosY2 = iPosY + 1;

                            if (iPosX2 >= width)
                            {
                                iPosX2--;
                            }

                            if (iPosY2 >= height)
                            {
                                iPosY2--;
                            }
                            dst[x, y] = (1.0d - dRestY) * ((1.0d - dRestX) * src[iPosX, iPosY] + dRestX * src[iPosX2, iPosY]) + dRestY * ((1.0d - dRestX) * src[iPosX, iPosY2] + dRestX * src[iPosX2, iPosY2]);
                        }
                    }
                }
            }

            /* Unoptimized version
            if (filter == ExtendedResizeFilter.LINEAR)
            {
                double a, b, c, d, restx, resty;
                double posx,posy;
                int iposx, iposy;


                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {


                        posx = (double)(x * (width - 1)) / (double)(newWidth - 1);// -0.5d;
                        posy = (double)(y * (height - 1)) / (double)(newHeight - 1);// -0.5d;
                        restx = posx % 1.0d;
                        resty = posy % 1.0d;
                        iposx = (int)posx;
                        iposy = (int)posy;

                        int iposx2 = iposx + 1;
                        int iposy2 = iposy + 1;

                            if (iposx2 >= width)
                            {
                                iposx2--;
                            }


                            if (iposy2 >= height)
                            {

                                iposy2--;
                            }
                                
                            a = src[iposx, iposy];
                            b = src[iposx2, iposy];
                            c = src[iposx, iposy2];
                            d = src[iposx2, iposy2];

                        double tmp1 = (1.0d - restx) * a + restx * b;
                        double tmp2 = (1.0d - restx) * c + restx * d;
                        dst[x, y] = (1.0d - resty) * tmp1 + resty * tmp2; 


                    }
                }
            }
            */
        }

        /// <summary>
        /// Resizes the src array with the declared filter and stores the result in the dst array. 
        /// </summary>
        /// <param name="src">source array</param>
        /// <param name="dst">destination array</param>
        /// <param name="width">width of source array</param>
        /// <param name="height">height of source array</param>
        /// <param name="newWidth">width of destination array</param>
        /// <param name="newHeight">height of destination array</param>
        /// <param name="filter">filter which should be used to resize the array</param>
        public static void ResizeArray2D(ref float[,] src, ref float[,] dst, int width, int height, int newWidth, int newHeight, ResizeFilter filter)
        {
            int minWidth = Math.Min(width, newWidth);
            int minHeight = Math.Min(height, newHeight);
            float tmp;

            if (filter == ResizeFilter.NONE_ZERO)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    for (int x = 0; x < minWidth; x++)
                    {
                        dst[x, y] = src[x, y];
                    }
                }
                for (int y = 0; y < minHeight; y++)
                {
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = 0.0f;
                    }
                }
                for (int y = minHeight; y < newHeight; y++)
                {
                    for (int x = 0; x < minWidth; x++)
                    {
                        dst[x, y] = 0.0f;
                    }
                }
                for (int y = minHeight; y < newHeight; y++)
                {
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = 0.0f;
                    }
                }
            }
            if (filter == ResizeFilter.NONE_CLAMP)
            {

                for (int y = 0; y < minHeight; y++)
                {
                    for (int x = 0; x < minWidth; x++)
                    {
                        dst[x, y] = src[x, y];
                    }
                }
                for (int y = 0; y < minHeight; y++)
                {
                    tmp = src[minWidth - 1, y];
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = tmp;
                    }
                }

                for (int x = 0; x < minWidth; x++)
                {
                    tmp = src[x, minHeight - 1];
                    for (int y = minHeight; y < newHeight; y++)
                    {

                        dst[x, y] = tmp;
                    }
                }

                tmp = src[minWidth - 1, minHeight - 1];
                for (int y = minHeight; y < newHeight; y++)
                {
                    for (int x = minWidth; x < newWidth; x++)
                    {
                        dst[x, y] = tmp;
                    }
                }
            }
            if (filter == ResizeFilter.POINT)
            {
                int iWidth2 = width - 1, iHeight2 = height - 1;
                int iNewWidth2 = newWidth - 1, iNewHeight2 = newHeight - 1;

                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        dst[x, y] = src[(x * iWidth2) / iNewWidth2, (y * iHeight2) / iNewHeight2];
                    }
                }
            }
            if (filter == ResizeFilter.LINEAR) // Does not work really good for downscaling
            {
                float fRestX, fRestY;
                float fPosX, fPosY;
                int iPosX, iPosY, iPosX2, iPosY2;
                int iWidth2 = width - 1, iHeight2 = height - 1;

                float fWidth2 = width - 1, fHeight2 = height - 1;
                float fNewWidth2 = newWidth - 1, fNewHeight2 = newHeight - 1;


                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        fPosX = (x * fWidth2) / fNewWidth2;
                        fPosY = (y * fHeight2) / fNewHeight2;
                        //fRestX = fPosX % 1.0d;
                        //fRestY = fPosY % 1.0d;
                        iPosX = (int)fPosX;
                        iPosY = (int)fPosY;
                        fRestX = fPosX - iPosX;
                        fRestY = fPosY - iPosY;

                        if ((iPosX < iWidth2) && (iPosY < iHeight2))
                        {
                            dst[x, y] = (1.0f - fRestY) * ((1.0f - fRestX) * src[iPosX, iPosY] + fRestX * src[iPosX + 1, iPosY]) + fRestY * ((1.0f - fRestX) * src[iPosX, iPosY + 1] + fRestX * src[iPosX + 1, iPosY + 1]);
                        }
                        else
                        {
                            iPosX2 = iPosX + 1;
                            iPosY2 = iPosY + 1;

                            if (iPosX2 >= width)
                            {
                                iPosX2--;
                            }

                            if (iPosY2 >= height)
                            {
                                iPosY2--;
                            }
                            dst[x, y] = (1.0f - fRestY) * ((1.0f - fRestX) * src[iPosX, iPosY] + fRestX * src[iPosX2, iPosY]) + fRestY * ((1.0f - fRestX) * src[iPosX, iPosY2] + fRestX * src[iPosX2, iPosY2]);
                        }
                    }
                }
            }
            /* Unoptimized version
            if (filter == ExtendedResizeFilter.LINEAR)
            {
                double a, b, c, d, restx, resty;
                double posx,posy;
                int iposx, iposy;


                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {


                        posx = (double)(x * (width - 1)) / (double)(newWidth - 1);// -0.5d;
                        posy = (double)(y * (height - 1)) / (double)(newHeight - 1);// -0.5d;
                        restx = posx % 1.0d;
                        resty = posy % 1.0d;
                        iposx = (int)posx;
                        iposy = (int)posy;

                        int iposx2 = iposx + 1;
                        int iposy2 = iposy + 1;

                            if (iposx2 >= width)
                            {
                                iposx2--;
                            }


                            if (iposy2 >= height)
                            {

                                iposy2--;
                            }
                                
                            a = src[iposx, iposy];
                            b = src[iposx2, iposy];
                            c = src[iposx, iposy2];
                            d = src[iposx2, iposy2];

                        double tmp1 = (1.0d - restx) * a + restx * b;
                        double tmp2 = (1.0d - restx) * c + restx * d;
                        dst[x, y] = (1.0d - resty) * tmp1 + resty * tmp2; 


                    }
                }
            }
            */
        }


        public static void ConvertFloatToInt(ref float[,] src, ref int[,] dst, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    dst[x, y] = (int)src[x, y];
                }
            }
        }

        public static void ConvertIntToFloat(ref int[,] src, ref float[,] dst, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    dst[x, y] = (float)src[x, y];
                }
            }
        }

        public static void CalcMinMax(ref int[,] array, int width, int height, ref int min, ref int max)
        {
            int iValue;
            max = int.MinValue;
            min = int.MaxValue;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    iValue = array[x, y];
                    if (iValue > max)
                    {
                        max = iValue;
                    }
                    iValue = array[x, y];
                    if (iValue < min)
                    {
                        min = iValue;
                    }
                }
            }
        }

        /*
        public static void CalcAvgAmp(ref int[,] array, int width, int height, ref int avg, ref int amp)
        {
            int iValue, iMid;
            amp = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    iValue = array[x, y];
                    if (Math.Abs(iValue) > amp)
                    {
                        amp = Math.Abs(iValue);
                    }
                    iMid += iValue;
                }
            }
            avg = iMid / (width * height);
        }
        */

        /*
        public static void RemoveDeadZone(ref int[,] array, int width, int height, ref int posDeadZone, ref int negDeadZone)
        {
            int iValue;
            posDeadZone = int.MaxValue;
            negDeadZone = int.MinValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    iValue = array[x, y];
                    if (iValue > 0 && iValue < posDeadZone)
                    {
                        posDeadZone = iValue;
                    }
                    if (iValue < 0 && iValue > negDeadZone)
                    {
                        negDeadZone = iValue;
                    }
                }
            }
            posDeadZone--;
            negDeadZone++;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (iValue > 0)
                    {
                        array[x, y] -= posDeadZone;
                    }
                    if (iValue < 0)
                    {
                        array[x, y] -= negDeadZone;
                    }
                }
            }
        }
        */

        /*
        public static void MultiplyCoefficients2D(ref double[,] arr, int width, int height, bool forward)
        {
            int w = 1;
            int h = 1;
            double multiply = 0.0d;
            int logw = MathEx.ILog2((uint)width); //(int)(Math.Log(width) / Math.Log(2) + 0.5d);
            int logh = MathEx.ILog2((uint)height); //(int)(Math.Log(height) / Math.Log(2) + 0.5d);

            if (logw == -1)
            {
                new ArgumentException("width must be a power of 2!");
            }
            if (logh == -1)
            {
                new ArgumentException("height must be a power of 2!");
            }

            for (int y = 1; y < logh; y++)
            {
                w = 1;
                for (int x = 1; x < logw; x++)
                {
                    //Box(w,h,w,h,x*y*6)
                    // 1.0 / (1.0 + Pow(Log(Sqr(w*w+h*h)/Sqr(2))/Log(2),2))

                    double dist = Math.Sqrt(w * w + h * h) / Math.Sqrt(2);  //Math.Sqrt(Math.Pow((double)width / (double)w, 2) + Math.Pow((double)height / (double)h, 2));      
                    double lg = Math.Log(dist) / Math.Log(2);

                    if (forward)
                    {
                        multiply = 256.0d - dist + 1.0d;  /// Math.Log(2)              Math.Sqrt(((double)width * height) / ((double)(w * h))); // 1 IS MUCH BETTER QUALITY, BUT WITH FORMULA BETTER COMPRESSION !!!!!! USE 1 BECAUSE WITH SAME COMPRESSION BETTER QUALITY
                    }
                    else
                    {
                        multiply = 1.0d / (256.0d - dist + 1.0d); // / Math.Log(2)
                    }

                    for (int ny = h - 1; ny < h * 2; ny++)
                    {
                        for (int nx = w - 1; nx < w * 2; nx++)
                        {
                            arr[nx, ny] = arr[nx, ny] * multiply;
                        }
                    }
                    w *= 2;
                }
                h *= 2;
            }
        }


        public static void ApplyQuantificationMatix2D(ref double[,] arr, int width, int height, ref double[,] quant, bool forward)
        {
            int w = 1;
            int h = 1;
            double multiply = 0.0d;
            int logw = MathEx.ILog2((uint)width); //(int)(Math.Log(width) / Math.Log(2) + 0.5d);
            int logh = MathEx.ILog2((uint)height); //(int)(Math.Log(height) / Math.Log(2) + 0.5d);

            if (logw == -1)
            {
                new ArgumentException("width must be a power of 2!");
            }
            if (logh == -1)
            {
                new ArgumentException("height must be a power of 2!");
            }

            for (int y = 1; y < logh; y++)
            {
                w = 1;
                for (int x = 1; x < logw; x++)
                {
                    //Box(w,h,w,h,x*y*6)

                    if (forward)
                    {
                        multiply = 1.0d / ((double)quant[x, y]); // 1 IS MUCH BETTER QUALITY, BUT WITH FORMULA BETTER COMPRESSION !!!!!! USE 1 BECAUSE WITH SAME COMPRESSION BETTER QUALITY
                    }
                    else
                    {
                        multiply = (double)quant[x, y];
                    }

                    for (int ny = h - 1; ny < h * 2; ny++)
                    {
                        for (int nx = w - 1; nx < w * 2; nx++)
                        {
                            arr[nx, ny] = Math.Truncate(arr[nx, ny] * multiply + 0.5d);
                        }
                    }
                    w *= 2;
                }
                h *= 2;
            }
        }
        */

    }
}
