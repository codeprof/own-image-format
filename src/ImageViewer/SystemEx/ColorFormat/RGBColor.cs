//Date: 2009-11-22
//Author: smoebius
//Description: Helper functions to manipulate RGB colors
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SystemEx.ColorFormat
{
    /// <summary>
    /// Helper functions to manipulate RGB/RGBA colors
    /// </summary>
    static public class RGBColor
    {
        /// <summary>
        /// Returns the 24Bit RGB color value for the three color channels(red, green and blue).
        /// </summary>
        static public int RGB(int r, int g, int b)
        {
            if (r < 0)
                r = 0;
            if (r > 255)
                r = 255;
            if (g < 0)
                g = 0;
            if (g > 255)
                g = 255;
            if (b < 0)
                b = 0;
            if (b > 255)
                b = 255;
            return b + (g << 8) + (r << 16) + (255 << 24);
        }

        /// <summary>
        /// Returns the 32Bit RGBA color value for the alpha and the three color channels(red, green and blue).
        /// </summary>
        static public int RGBA(int r, int g, int b, int a)
        {
            if (r < 0)
                r = 0;
            if (r > 255)
                r = 255;
            if (g < 0)
                g = 0;
            if (g > 255)
                g = 255;
            if (b < 0)
                b = 0;
            if (b > 255)
                b = 255;
            if (a < 0)
                a = 0;
            if (a > 255)
                a = 255;
            return b + (g << 8) + (r << 16) + (a << 24);
        }

        /// <summary>
        /// Returns the red component(0-255) of the declared 24Bit/32bit color value
        /// </summary>
        static public int GetRed(int rgb)
        {
            return (rgb >> 16) & 0xFF;
        }

        /// <summary>
        /// Returns the blue component(0-255) of the declared 24Bit/32bit color value
        /// </summary>
        static public int GetBlue(int rgb)
        {
            return (rgb) & 0xFF;
        }

        /// <summary>
        /// Returns the green component(0-255) of the declared 24Bit/32bit color value
        /// </summary>
        static public int GetGreen(int rgb)
        {
            return (rgb >> 8) & 0xFF;
        }

        /// <summary>
        /// Returns the alpha component(0-255) of the declared 32Bit color value
        /// </summary>
        static public int GetAlpha(int rgb)
        {
            return (rgb >> 24) & 0xFF;
        }

        
        /// <summary>
        /// Fills the declared array with the content of bitmap
        /// </summary>
        /// <param name="bmp">bitmap object</param>
        /// <param name="rgbArray">reference to the an int[,] array</param>
        public static void FillRGBArrayFromBitmap(System.Drawing.Bitmap bmp, ref int[,] rgbArray)
        {
            Rectangle rect;
            System.Drawing.Imaging.BitmapData bmpData;
            int[] rgbLine;
            IntPtr pScan0;
            IntPtr ptr;
            int bytesPerLine;
            int offset;

            if (bmp == null)
            {
                throw new NullReferenceException("Bitmap cannot be null!");
            }
            if (rgbArray == null)
            {
                throw new NullReferenceException("rgbArray cannot be null!");
            }

            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ptr = bmpData.Scan0; // start address of the image
            bytesPerLine = bmpData.Stride;// bytes per line
            offset = 0;
            rgbLine = new int[bmp.Width];

            if (bytesPerLine < 0) // image is top down
            {
                pScan0 = (IntPtr)((long)bmpData.Scan0 - bytesPerLine * bmp.Height); // last address instead of first
            }
            else
            {
                pScan0 = bmpData.Scan0;
            }

            for (int y = 0; y < bmp.Height; y++)
            {
                ptr = (IntPtr)((long)pScan0 + (long)offset); // better use long instead of int (otherwise there could be problems when using as 64bit application)
                Marshal.Copy(ptr, rgbLine, 0, bmp.Width);
                // copy the data into the 2d array (at least faster than reading it pixel by pixel)
                for (int x = 0; x < bmp.Width; x++)
                {
                    rgbArray[x, y] = rgbLine[x];
                }
                offset += bytesPerLine;
            }
            bmp.UnlockBits(bmpData);
        }
        

        public static BitmapSource CreateBitmapFromRGBArray(ref int[,] rgbArray, int width, int height)
        {
            int rawStride = width* 4;//(width * PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
            int iIndex = 0;
            int[] rawImage = new int[width * height];

            for (int y=0; y < height; y++)
            {
                for (int x=0; x < width; x++)
                {
                    rawImage[iIndex] = rgbArray[x, y];
                    iIndex++;
                }
            }

            // Create a BitmapSource.
            BitmapSource bitmap = BitmapSource.Create(width, height,
                96, 96, PixelFormats.Bgr32, null,
                rawImage, rawStride);

            return bitmap;
        }
   
        /*
        /// <summary>
        /// Creates a bitmap with the content of array
        /// </summary>
        /// <param name="rgbArray">reference to the an int[,] array</param>
        /// <param name="width">width of the bitmap</param>
        /// <param name="height">height of bitmap</param>
        /// <returns>Bitmap object</returns>
        public static Bitmap CreatesBitmapFromRGBArray(ref int[,] rgbArray, int width, int height)
        {
            Bitmap bmp;
            Rectangle rect;
            BitmapData bmpData;
            int[] rgbLine;
            IntPtr pScan0;
            IntPtr ptr;
            int bytesPerLine;
            int offset;

            if (rgbArray == null)
            {
                throw new NullReferenceException("rgbArray cannot be null!");
            }

            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            rect = new Rectangle(0, 0, width, height);
            bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            ptr = bmpData.Scan0; // start address of the image
            bytesPerLine = bmpData.Stride; // bytes per line
            offset = 0;
            rgbLine = new int[width];

            if (bytesPerLine < 0) // image is top down
            {
                pScan0 = (IntPtr)((long)bmpData.Scan0 - bytesPerLine * height); // last address instead of first
            }
            else
            {
                pScan0 = bmpData.Scan0;
            }

            for (int y = 0; y < height; y++)
            {
                ptr = (IntPtr)((long)pScan0 + (long)offset); // better use long instead of int (otherwise there could be problems when using as 64bit application)
                for (int x = 0; x < bmp.Width; x++)
                {
                    rgbLine[x] = rgbArray[x, y];
                }
                Marshal.Copy(rgbLine, 0, ptr, width);

                offset += bytesPerLine;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        */

        /// <summary>
        /// Loads an image file into a 2 dimensional RGB array 
        /// </summary>
        /// <param name="file">path to a .BMP, .GIF, .EXIG, .JPG, .PNG or .TIFF file</param>
        /// <param name="width">variable to store the width of the image</param>
        /// <param name="height">variable to store the width of the image</param>
        /// <returns>returns a two dimensional interger array with the pixel of the image in rgb format</returns>
        public static int[,] LoadImageAsRGBArray(string file, ref int width, ref int height)
        {
            Bitmap bmp;
            bmp = new Bitmap(file, true);
            int[,] rgbArray = new int[bmp.Width, bmp.Height];
            FillRGBArrayFromBitmap(bmp, ref rgbArray);
            width = bmp.Width;
            height = bmp.Height;
            return rgbArray;
        }

        /*
        /// <summary>
        /// Saves an image file from a 2 dimensional RGB array 
        /// </summary>
        /// <param name="file">path to a .BMP, .GIF, .EXIG, .JPG, .PNG or .TIFF file</param>
        /// <param name="rgbArray">two dimensional integer array</param>
        /// <param name="width">width of the image</param>
        /// <param name="height">height of the image</param>
        /// <param name="fmt">format of the image</param>
        public static void SaveRGBArrayAsImage(string file, ref int[,] rgbArray, int width, int height, System.Drawing.Imaging.ImageFormat fmt)
        {
            Bitmap bmp = CreateBitmapFromRGBArray(ref rgbArray, width, height);
            bmp.Save(file, fmt);
        }

        /// <summary>
        /// Saves an image file from a 2 dimensional RGB array as .png file
        /// </summary>
        /// <param name="file">path to the file</param>
        /// <param name="rgbArray">two dimensional integer array</param>
        /// <param name="width">width of the image</param>
        /// <param name="height">height of the image</param>
        public static void SaveRGBArrayAsImage(string file, ref int[,] rgbArray, int width, int height)
        {
            SaveRGBArrayAsImage(file, ref rgbArray, width, height, System.Drawing.Imaging.ImageFormat.Png);
        }
        */
    }
}
