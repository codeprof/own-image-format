//Date: 2009-11-22
//Author: smoebius
//Description: Helper functions to manipulate YCgCo colors
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemEx.ColorFormat
{
    /// <summary>
    /// Helper functions to manipulate YCgCoColor colors
    /// </summary>
    class YCgCoColor
    {
        //RGB -> YCgCo
        //          R,    G,      B
        //Y  =   0.25, 0.50,   0.25
        //Cg =  -0.25, 0.50,  -0.25
        //Co =   0.50, 0.00,  -0.50

        //YCgCo -> RGB
        //       Y,   Cg,   Co
        //R =  1.0,  1.0, -1.0
        //G =  1.0,  1.0,  0.0
        //B =  1.0, -1.0, -1.0

        /// <summary>
        /// Returns the 24Bit YCgCo color value for the three color channels.
        /// </summary>
        /// <param name="y">Y channel(0-255)</param>
        /// <param name="cg">Cg channel(0-255)</param>
        /// <param name="co">Co channel(0-255)</param>
        /// <returns></returns>
        static public int YCgCo(int y, int cg, int co)
        {
            if (y < 0)
                y = 0;
            if (cg < 0)
                cg = 0;
            if (co < 0)
                co = 0;
            if (y > 255)
                y = 255;
            if (cg > 255)
                cg = 255;
            if (co > 255)
                co = 255;

            return y + (cg << 8) + (co << 16);
        }

        /// <summary>
        /// Converts a 24bit RGB value to the YCgCo format
        /// </summary>
        /// <param name="rgb">24bit RGB color value</param>
        /// <returns>24bit YCgCo color value</returns>
        static public int RGBToYCgCo(int rgb)
        {
            int b = rgb & 255;
            int g = (rgb >> 8) & 255;
            int r = (rgb >> 16) & 255;

            int y = (r >> 2) + ((g + b) >> 1);
            int cg = -((r + b) >> 2) + (g >> 1) + 128;
            int co = ((r - b) >> 1) + 128;

            /* should not be necessary
                if (Y<0)
                    Y=0;
                if (V<0)
                    V=0;
                if (U<0)
                    U=0;
                if (Y>255)
                    Y=255;
                if (V>255)
                    V=255;
                if (U>255)
                    U=255;
            */

            return y + (cg << 8) + (co << 16);
        }



        /// <summary>
        /// converts a 2d integer array from 24 bit RGB format to the YCgCo format 
        /// </summary>
        /// <param name="rgbArray">pointer to a 2d integer array with 24 bit rgb data</param>
        /// <param name="Width">width of the image</param>
        /// <param name="Height">height of the image</param>
        /// <param name="outY">pointer to a float[,] array with the same dimensions which will be filled with Y component</param>
        /// <param name="outCg">pointer to a float[,] array with the same dimensions which will be filled with Cg component</param>
        /// <param name="outCo">pointer to a float[,] array with the same dimensions which will be filled with Co component</param>
        static public void RGBArrayToYCgCoArrays(ref int[,] rgbArray, int Width, int Height, ref float[,] outY, ref float[,] outCg, ref float[,] outCo)
        {
            int col; // 24 bit color value
            int r, g, b; // red, green and blue color channel

            if (rgbArray == null)
            {
                throw new ArgumentException("rgbArray cannot be null!");
            }
            if (outY == null)
            {
                throw new ArgumentException("outY cannot be null!");
            }
            if (outCg == null)
            {
                throw new ArgumentException("outCg cannot be null!");
            }
            if (outCo == null)
            {
                throw new ArgumentException("outCo cannot be null!");
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    col = rgbArray[x, y];
                    b = col & 255;
                    g = (col >> 8) & 255;
                    r = (col >> 16) & 255;

                    outY[x, y] = (float)( ((r+b) >> 2) + (g >> 1));
                    outCg[x, y] = (float)(-((r + b) >> 2) + (g >> 1)) + 128.0f;
                    outCo[x, y] = (float)( (r - b) >> 1) + 128.0f;
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
                    outY[x, y] = (float)((((col >> 16) & 255) + (col & 255)) >> 2 + ((col >> 8) & 255) >> 1);
                }
            }
        }




        
        /*
        /// <summary>
        /// converts a 2d integer array from 24 bit RGB format to the YCgCo format 
        /// </summary>
        /// <param name="rgbArray">2d integer array with 24 bit rgb data</param>
        /// <param name="Width">width of the array</param>
        /// <param name="Height">height of the array</param>
        /// <param name="outY">pointer to a float[,] array width the same dimensions</param>
        /// <param name="outCg">pointer to a float[,] array width the same dimensions</param>
        /// <param name="outCo">pointer to a float[,] array width the same dimensions</param>
        static public void RGBArrayToYCgCoArrays(ref int[,] rgbArray, int Width, int Height, ref double[,] outY, ref double[,] outCg, ref double[,] outCo)
        {
            if (outY != null && outCg != null && outCo != null)
            {

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        int B = rgbArray[x, y] & 255;
                        int G = (rgbArray[x, y] >> 8) & 255;
                        int R = (rgbArray[x, y] >> 16) & 255;

                        outY[x, y] = (double)(R >> 2 + (G + B) >> 1);
                        outCg[x, y] = (double)(-((R + B) >> 2) + (G >> 1) + 128);
                        outCo[x, y] = (double)((R - B) >> 1 + 128);
                        //outY[x, y] = (double)(R / 4 + G / 2 + B / 2);
                        //outCg[x, y] = (double)(-R / 4 + G / 2 - B / 4 + 128);
                        //outCo[x, y] = (double)(R / 2 - B / 2 + 128);
                        
                    }
                }
            }
            else if (outY != null && outCg == null && outCo == null)
            {
                //Optimize for this special case...
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        int R = rgbArray[x, y] & 255;
                        int G = (rgbArray[x, y] >> 8) & 255;
                        int B = (rgbArray[x, y] >> 16) & 255;

                        outY[x, y] = (double)(R >> 2 + (G + B) >> 1);
                    }
                }
            }
            else
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        int R = rgbArray[x, y] & 255;
                        int G = (rgbArray[x, y] >> 8) & 255;
                        int B = (rgbArray[x, y] >> 16) & 255;

                        if (outY != null)
                        {
                            outY[x, y] = (double)(R / 4 + G / 2 + B / 2);
                        }
                        if (outCg != null)
                        {
                            outCg[x, y] = (double)(-R / 4 + G / 2 - B / 4 + 128);
                        }
                        if (outCo != null)
                        {
                            outCo[x, y] = (double)(R / 2 - B / 2 + 128);
                        }
                    }
                }
            }
        }
        */




        static public void YCgCoArraysToRGBArray(ref float[,] inY, ref float[,] inCg, ref float[,] inCo, int Width, int Height, ref int[,] rgbArray)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float Y = inY[x, y];
                    float Cg = inCg[x, y];
                    float Co = inCo[x, y];

                    // 1.0,  1.0, -1.0
                    // 1.0,  1.0,  0.0
                    // 1.0, -1.0, -1.0

                    Cg -= 127.5f;
                    Co -= 127.5f;
                    int R = (int)(Y + Cg - Co + 0.5f);
                    int G = (int)(Y + Cg      + 0.5f);
                    int B = (int)(Y - Cg - Co + 0.5f);

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
                    if (R < 0)
                        R = 0;
                    if (G < 0)
                        G = 0;
                    if (B < 0)
                        B = 0;
                    if (R > 255)
                        R = 255;
                    if (G > 255)
                        G = 255;
                    if (B > 255)
                        B = 255;

                    rgbArray[x, y] = B + (G << 8) + (R << 16) + (255 << 24);
                }
            }
        }


        static public int GetYValue(int ycgco)
        {
            return ycgco & 255;
        }
        static public int GetCgValue(int ycgco)
        {
            return (ycgco >> 8) & 255;
        }

        static public int GetCoValue(int ycgco)
        {
            return (ycgco >> 16) & 255;
        }

        /// <summary>
        /// Converts a color value from the YCgCo format to the RGB format
        /// </summary>
        /// <param name="ycgco">YCgCo value</param>
        /// <returns>RGB value</returns>
        static public int YCgCoToRGB(int ycgco)
        {
            int y = ycgco & 255;
            int cg = (ycgco >> 8) & 255;
            int co = (ycgco >> 16) & 255;

            cg -= 128;
            co -= 128;
            int r = (y + cg - co);
            int g = (y + cg);
            int b = (y - cg - co);

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
