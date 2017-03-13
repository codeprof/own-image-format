//Date: 2009-11-22
//Author: smoebius
//Description: Helper functions to manipulate YCbCr colors
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//The author is not liable for any damage caused by this software.
//Licenced under MIT licence
using System;

namespace SystemEx.ColorFormat
{
    /// <summary>
    /// Helper functions to manipulate YCbCr colors
    /// </summary>
    public static class YCbCrColor
    {
        /// <summary>
        /// Returns the 24Bit YCbCr color value for the three color channels.
        /// </summary>
        /// <param name="y">Y channel(0-255)</param>
        /// <param name="cb">Cb channel(0-255)</param>
        /// <param name="cr">Cr channel(0-255)</param>
        /// <returns>24bit YCbCr color value</returns>
        static public int YCbCr(int y, int cb, int cr)
        {
            if (y < 0)
                y = 0;
            if (cb < 0)
                cb = 0;
            if (cr < 0)
                cr = 0;
            if (y > 255)
                y = 255;
            if (cb > 255)
                cb = 255;
            if (cr > 255)
                cr = 255; 
            return y + (cb << 8) + (cr << 16);
        }

        /// <summary>
        /// Converts a 24bit RGB value to the YCbCr format
        /// </summary>
        /// <param name="rgb">24bit RGB color value</param>
        /// <returns>24bit YCbCr color value</returns>
        static public int RGBToYCbCr(int rgb)
        {
            float r = (float)(rgb & 255);
            float g = (float)((rgb >> 8) & 255);
            float b = (float)((rgb >> 16) & 255);

            int y  = (int)( 0.2990f * r + 0.5870f * g + 0.1140f * b + 0.5d);
            int cb = (int)(-0.1687f * r - 0.3313f * g + 0.5000f * b + 127.5d + 0.5d);
            int cr = (int)( 0.5000f * r - 0.4187f * g - 0.0813f * b + 127.5d + 0.5d);

            /* should not be necessary?
                if (y<0)
                    y=0;
                if (v<0)
                    v=0;
                if (u<0)
                    u=0;
                if (y>255)
                    y=255;
                if (v>255)
                    v=255;
                if (u>255)
                    u=255;

             */
            
            return y + (cb << 8) + (cr << 16); 
        }

        /// <summary>
        /// converts a 2d integer array from 24 bit RGB format to the YCbCr format 
        /// </summary>
        /// <param name="rgbArray">pointer to a 2d integer array with 24 bit rgb data</param>
        /// <param name="Width">width of the image</param>
        /// <param name="Height">height of the image</param>
        /// <param name="outY">pointer to a float[,] array with the same dimensions which will be filled with Y component</param>
        /// <param name="outCb">pointer to a float[,] array with the same dimensions which will be filled with Cb component</param>
        /// <param name="outCr">pointer to a float[,] array with the same dimensions which will be filled with Cr component</param>
        static public void RGBArrayToYCbCrArrays(ref int[,] rgbArray, int Width, int Height, ref float[,] outY, ref float[,] outCb, ref float[,] outCr)
        {
            int col; // 24 bit color value
            float r, g, b; // red, green and blue color channel

            if (rgbArray == null)
            {
                throw new ArgumentException("rgbArray cannot be null!");
            }
            if (outY == null)
            {
                throw new ArgumentException("outY cannot be null!");
            }
            if (outCb == null)
            {
                throw new ArgumentException("outCb cannot be null!");
            }
            if (outCr == null)
            {
                throw new ArgumentException("outCr cannot be null!");
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    col = rgbArray[x, y];
                    b = col & 255;
                    g = (col >> 8) & 255;
                    r = (col >> 16) & 255;

                    outY[ x, y] = ( 0.2990f * r + 0.5870f * g + 0.1140f * b + 0.5f);
                    outCb[x, y] = (-0.1687f * r - 0.3313f * g + 0.5000f * b + 127.5f + 0.5f);
                    outCr[x, y] = ( 0.5000f * r - 0.4187f * g - 0.0813f * b + 127.5f + 0.5f);
                }
            }
        }

        /// <summary>
        /// converts a 2d integer array from 24 bit RGB format to an 2d float array with the luminance component of the RGB values.
        /// </summary>
        /// <param name="rgbArray">pointer to a 2d integer array with 24 bit rgb data</param>
        /// <param name="Width">width of the image</param>
        /// <param name="Height">height of the image</param>
        /// <param name="outY">pointer to a float[,] array with the same dimensions which will be filled with the luminance component</param>
        static public void RGBArrayToYArray(ref int[,] rgbArray, int Width, int Height, ref float[,] outY)
        {
            int col; // 24 bit color value
            if (rgbArray == null)
            {
                throw new ArgumentException("rgArray cannot be null!");
            }
            if (outY == null)
            {
                throw new ArgumentException("outY cannot be null!");
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    col = rgbArray[x, y];
                    outY[x, y] = (0.2990f * (float)(col & 255) + 0.5870f * (float)((col >> 8) & 255) + 0.1140f * (float)((col >> 16) & 255) + 0.5f);
                }
            }
        }

        /// <summary>
        /// converts the Y,Cb,Cr float arrays of a image to a 24 bit RGB integer array
        /// </summary>
        /// <param name="inY">pointer to a float[,] array with the Y component</param>
        /// <param name="inCb">pointer to a float[,] array with the Cb component</param>
        /// <param name="inCr">pointer to a float[,] array with the Cr component</param>
        /// <param name="Width">width of the image</param>
        /// <param name="Height">height of the image</param>
        /// <param name="rgbArray">pointer to a 2d integer array which will be filled with the 24 bit RGB values</param>
        static public void YCbCrArraysToRGBArray(ref float[,] inY, ref float[,] inCb, ref float[,] inCr, int Width, int Height, ref int[,] rgbArray)
        {
            float y, cb, cr;
            int r, g, b;

            if (inY == null)
            {
                throw new ArgumentException("inY cannot be null!");
            }
            if (inCb == null)
            {
                throw new ArgumentException("inCb cannot be null!");
            }
            if (inCr == null)
            {
                throw new ArgumentException("inCr cannot be null!");
            }
            if (rgbArray == null)
            {
                throw new ArgumentException("rgbArray cannot be null!");
            }

            for (int posY = 0; posY < Height; posY++)
            {
                for (int posX = 0; posX < Width; posX++)
                {
                    y  = inY[posX, posY];
                    cb = inCb[posX, posY] - 127.5f;
                    cr = inCr[posX, posY] - 127.5f;

                    r = (int)(y + 1.40200f * cr + 0.5f);
                    g = (int)(y - 0.34414f * cb - 0.71417f * cr + 0.5f);
                    b = (int)(y + 1.77200f * cb + 0.5f);

                     /*
                     if (Y==255)
                     {
                         R=255;
                         G=255; 
                         B=255;
                     }

                     if (Y==0)
                     {
                         R=0;
                         G=0;
                         B=0;
                     }
                     */
                    if (r < 0)
                        r = 0;
                    if (g < 0)
                        g = 0;
                    if (b < 0)
                        b = 0;
                    if (r > 255)
                        r = 255;
                    if (g > 255)
                        g = 255;
                    if (b > 255)
                        b = 255;

                    rgbArray[posX, posY] = b + (g << 8) + (r << 16) + (255 << 24);
                }
            }
        }

        /// <summary>
        /// returns the Y channel of YCbCr color value
        /// </summary>
        /// <param name="ycbcr">YCbCr color value</param>
        /// <returns>returns the Y channel</returns>
        static public int GetYValue(int ycbcr)
        {
            return ycbcr & 255;
        }

        /// <summary>
        /// returns the Cb channel of YCbCr color value
        /// </summary>
        /// <param name="ycbcr">YCbCr color value</param>
        /// <returns>returns the Cb channel</returns>
        static public int GetCbValue(int ycbcr)
        {
            return (ycbcr >> 8) & 255;
        }

        /// <summary>
        /// returns the Cr channel of YCbCr color value
        /// </summary>
        /// <param name="ycbcr">YCbCr color value</param>
        /// <returns>returns the Cr channel</returns>
        static public int GetCrValue(int ycbcr)
        {
            return (ycbcr >> 16) & 255;
        }

        /// <summary>
        /// Converts a 24bit YCbCr value to the RGB format
        /// </summary>
        /// <param name="ycbcr">24bit YCbCr color value</param>
        /// <returns>24bit RGB color value</returns>
        static public int YCbCrToRGB(int ycbcr)
        {
            float y = ycbcr & 255;
            float cb = (ycbcr >> 8) & 255;
            float cr = (ycbcr >> 16) & 255;

            cb -= 127.5f;
            cr -= 127.5f;
            int r = (int)(y + 1.40200f * cr                  + 0.5f);
            int g = (int)(y - 0.34414f * cb -  0.71417f * cr + 0.5f);
            int b = (int)(y + 1.77200f * cb                  + 0.5f);
            /*
            if (Y==255)
            {
                R=255;
                G=255; 
                B=255;
            }
            if (Y==0)
            {
                R=0;
                G=0;
                B=0;
            }
            */
            if (r < 0)
                r = 0;
            if (g < 0)
                g = 0;
            if (b < 0)
                b = 0;
            if (r > 255)
                r = 255;
            if (g > 255)
                g = 255;
            if (b > 255)
                b = 255;

            return b + (g << 8) + (r << 16) + (255 << 24);
        }


    }
}
