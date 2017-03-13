//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//The author is not liable for any damage caused by this software.
//Licenced under MIT licence
using System;
using System.Collections.Generic;
using SystemEx.ColorFormat;
using SystemEx.MathTransform;
using SystemEx.MathEx;
using System.Text;
using SystemEx;
using SystemEx.FileIO;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SystemEx.ImageFormat
{
    public static class ImageCodec
    {

        public static int[,] LoadFromFile(string sFile,ref int width, ref int height)
        {
            FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            byte[] decompressed = Memory.DecompressBuffer(ref buffer);
            ms.Write(decompressed, 0, (int)decompressed.Length);
            ms.Seek(0, 0);
            fs.Close();
            BinaryReader br = new BinaryReader(ms);
            //int width, height;
            int newWidth, newHeight;
            width = br.ReadInt32();
            height = br.ReadInt32();
            int quality = br.ReadInt32();
            newWidth = IntegerMath.ToPowerOf2(width);
            newHeight = IntegerMath.ToPowerOf2(height);

            int[,] imgOrg = new int[width, height];
            float[,] imgY = new float[newWidth, newHeight];
            float[,] imgCg = new float[newWidth, newHeight];
            float[,] imgCo = new float[newWidth, newHeight];
            int[,] imgIY = new int[newWidth, newHeight];
            int[,] imgICg = new int[newWidth, newHeight];
            int[,] imgICo = new int[newWidth, newHeight];

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    imgIY[x, y]  = br.ReadInt32();
                    imgICg[x, y] = br.ReadInt32();
                    imgICo[x, y] = br.ReadInt32();
                }
            }
           
            br.Close();
            ArrayHelper2D.ConvertIntToFloat(ref imgIY, ref imgY, newWidth, newHeight);
            ArrayHelper2D.ConvertIntToFloat(ref imgICg, ref imgCg, newWidth, newHeight);
            ArrayHelper2D.ConvertIntToFloat(ref imgICo, ref imgCo, newWidth, newHeight);
            Biorthogonal53Wavelet2D wavelet = new Biorthogonal53Wavelet2D(newWidth, newHeight, 8, 8, true);

            //revert simple quantification
            float quant = (float)((100.0f - quality) + 1.0f);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    imgY[x, y] *= quant;
                    imgCg[x, y] *= quant;
                    imgCo[x, y] *= quant;
                }
            }

            wavelet.BackTransformIsotropic2D(ref imgY);
            wavelet.BackTransformIsotropic2D(ref imgCg);
            wavelet.BackTransformIsotropic2D(ref imgCo);
            YCbCrColor.YCbCrArraysToRGBArray(ref imgY, ref imgCg, ref imgCo, width, height, ref imgOrg);           
            return imgOrg;
            //return RGBColor.CreateBitmapFromRGBArray(ref imgOrg, width, height);
        }


        public static void SaveAsFile(string sFile, ref int[,] rgbArray, int width, int height, int quality)
        {
            int size = width * height;
            int numRemainingY = quality * size / 100;
            int numRemainingCg = quality * size / 25;
            int numRemainingCo = quality * size / 25;

            int newWidth, newHeight;
            // int[,] rgbArray = new int[width, height];
            // RGBColor.FillRGBArrayFromBitmap(bmp, ref rgbArray);
            FileStream fs = new FileStream(sFile, FileMode.Create, FileAccess.Write, FileShare.None);
            MemoryStream ms = new MemoryStream();
            newWidth = IntegerMath.ToPowerOf2(width);
            newHeight = IntegerMath.ToPowerOf2(height);

            float[,] imgOrgY = new float[width, height];
            float[,] imgOrgCg = new float[width, height];
            float[,] imgOrgCo = new float[width, height];
            float[,] imgY = new float[newWidth, newHeight];
            float[,] imgCg = new float[newWidth, newHeight];
            float[,] imgCo = new float[newWidth, newHeight];
            int[,] imgIY = new int[newWidth, newHeight];
            int[,] imgICg = new int[newWidth, newHeight];
            int[,] imgICo = new int[newWidth, newHeight];
            //YCgCoColor.RGBArrayToYCgCoArrays(ref rgbArray, width, height, ref imgOrgY, ref imgOrgCg, ref imgOrgCo);
            YCbCrColor.RGBArrayToYCbCrArrays(ref rgbArray, width, height, ref imgOrgY, ref imgOrgCg, ref imgOrgCo);
            ArrayHelper2D.ResizeArray2D(ref imgOrgY, ref imgY, width, height, newWidth, newHeight, ArrayHelper2D.ResizeFilter.NONE_CLAMP);
            ArrayHelper2D.ResizeArray2D(ref imgOrgCg, ref imgCg, width, height, newWidth, newHeight, ArrayHelper2D.ResizeFilter.NONE_CLAMP);
            ArrayHelper2D.ResizeArray2D(ref imgOrgCo, ref imgCo, width, height, newWidth, newHeight, ArrayHelper2D.ResizeFilter.NONE_CLAMP);
            Biorthogonal53Wavelet2D wavelet = new Biorthogonal53Wavelet2D(newWidth, newHeight, 8, 8, true);
            wavelet.TransformIsotropic2D(ref imgY);
            wavelet.TransformIsotropic2D(ref imgCg);
            wavelet.TransformIsotropic2D(ref imgCo);
            ArrayHelper2D.RemoveSmallestCoefficients2D(ref imgY, newWidth, newHeight, numRemainingY);
            ArrayHelper2D.RemoveSmallestCoefficients2D(ref imgCg, newWidth, newHeight, numRemainingCg);
            ArrayHelper2D.RemoveSmallestCoefficients2D(ref imgCo, newWidth, newHeight, numRemainingCo);

            //simple quantification
            float quant = 1.0f / (float)((100.0f-quality) + 1.0f);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    imgY[x, y] *= quant;
                    imgCg[x, y] *= quant;
                    imgCo[x, y] *= quant;
                }
            }

            ArrayHelper2D.ConvertFloatToInt(ref imgY, ref imgIY, newWidth, newHeight);
            ArrayHelper2D.ConvertFloatToInt(ref imgCg, ref imgICg, newWidth, newHeight);
            ArrayHelper2D.ConvertFloatToInt(ref imgCo, ref imgICo, newWidth, newHeight);
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(width);
            writer.Write(height);
            writer.Write(quality);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    writer.Write(imgIY[x, y]);
                    writer.Write(imgICg[x, y]);
                    writer.Write(imgICo[x, y]);
                }
            }
            writer.Close();
            byte[] buffer = ms.GetBuffer();
            byte[] compressed = Memory.CompressBuffer(ref buffer);
            fs.Write(compressed, 0, compressed.Length);
            fs.Close();

        }


    }
}
