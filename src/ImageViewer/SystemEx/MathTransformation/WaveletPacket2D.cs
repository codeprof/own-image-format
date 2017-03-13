//Date: 2009-12-21
//Author: smoebius
//Description: abstract 2D wavelet class which can be used for different wavelet implementations
//Copyright: 2009 Stefan Moebius
using System;
using SystemEx.MathEx;
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.

namespace SystemEx.MathTransform
{
    /// <summary>
    /// abstract 2D wavelet class which can be used for different wavelet implementations
    /// </summary>
    public abstract class WaveletPacket2D
    {
        #region protected attributes
        /// <summary>
        /// Width of the wavelet transformation
        /// </summary>
        protected int iWidth = 0;
        /// <summary>
        /// Height of the wavelet transformation
        /// </summary>
        protected int iHeight = 0;
        /// <summary>
        /// min. size for horizontal transformation
        /// </summary>
        protected int szMinHorz = 0;
        /// <summary>
        /// min. size for vertical transformation
        /// </summary>
        protected int szMinVert = 0;
        /// <summary>
        /// The allowed minimum for szMinHorz and szMinVert
        /// </summary>
        protected int iAllowedMinSize = 0;
        /// <summary>
        /// scale factor
        /// </summary>
        protected bool bScale = false;
        /// <summary>
        /// temporary buffer which can be used while the transformation
        /// </summary>
        protected float[] tmp = null;
        /// <summary>
        /// scale factor
        /// </summary>
        protected const float K = 1.4142135623730950488016f;
        /// <summary>
        /// inverse scale factor
        /// </summary>
        protected const float IK = 1.0f / 1.4142135623730950488016f;
        #endregion
        #region private methods
        private void CheckPowerOf2(string sName, int iValue)
        {
            if (IntegerMath.IsPowerOf2(iValue) == false)
            {
                throw new ArgumentException(sName + "(=" + iValue.ToString() + ") must be a power of two(like 2,4,8,32,64,...)!");
            }
        }
        private void CheckArguments(int offsetX, int offsetY, int width, int height)
        {
            if (offsetX < 0 || offsetX > iWidth)
            {           
                throw new ArgumentException("offsetX(=" + offsetX + ") must be between 0 and " + iWidth);
            }
            if (offsetY < 0 || offsetY > iHeight)
            {
                throw new ArgumentException("offsetY(=" + offsetY + ") must be between 0 and " + iHeight);
            }
            if (width < 0 || width > iWidth)
            {
                throw new ArgumentException("width(=" + width + ") must be between 0 and " + iWidth);
            }
            if (height < 0 || height > iHeight)
            {
                throw new ArgumentException("height(=" + height + ") must be between 0 and " + iHeight);
            }
            if ((offsetX + width > iWidth) || (offsetY + height > iHeight))
            {
                string sArea = offsetX + "," + offsetY + "," + (offsetX + width) + "," + (offsetY + height);
                string sMaxArea = "0,0," + iWidth + "," + iHeight;
                throw new ArgumentException("area (" + sArea + ") is out of range (" + sMaxArea + ")");
            }
        }
        private void Init(int MinHorizontalSize, int MinVerticalSize, int AllowedMinSize, int PowerOf2Width, int PowerOf2Height, bool Scale)
        {
            CheckPowerOf2("min. horizontal size", MinHorizontalSize);
            CheckPowerOf2("min. vertical size", MinVerticalSize);
            CheckPowerOf2("AllowedMinSize", AllowedMinSize);
            CheckPowerOf2("PowerOf2Width", PowerOf2Width);
            CheckPowerOf2("PowerOf2Height", PowerOf2Height);
            if (MinHorizontalSize < AllowedMinSize)
            {
                throw new ArgumentException("MinHorizontalSize cannot be smaller than " + AllowedMinSize);
            }
            if (MinVerticalSize < AllowedMinSize)
            {
                throw new ArgumentException("MinVerticalSize cannot be smaller than " + AllowedMinSize);
            }
            if (PowerOf2Width < MinHorizontalSize)
            {
                throw new ArgumentException("PowerOf2Width(=" + PowerOf2Width + ") must be greater or equal to " + MinHorizontalSize);
            }
            if (PowerOf2Height < MinVerticalSize)
            {
                throw new ArgumentException("PowerOf2Height(=" + PowerOf2Height + ") must be greater or equal to " + MinVerticalSize);
            }
            iWidth = PowerOf2Width;
            iHeight = PowerOf2Height;
            szMinHorz = MinHorizontalSize;
            szMinVert = MinVerticalSize;
            iAllowedMinSize = AllowedMinSize;
            bScale = Scale;
            tmp = new float[Math.Max(iWidth, iHeight)]; // is used for both horizontal and vertical transformation to reduce the needed memory
        }
        #endregion
        #region protected methods
        /// <summary>
        /// Performs a horizontal transformation
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        /// <param name="row">index of the row which should be transformed</param>
        /// <param name="start">start position</param>
        /// <param name="count">number of entries to transform</param>
        virtual protected void TransformRow(ref float[,] array, int row, int start, int count)
        {
            //will be overwritten by method of child class...
        }
        /// <summary>
        /// Performs a vertical transformation
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        /// <param name="column">index of the column which should be transformed</param>
        /// <param name="start">start position</param>
        /// <param name="count">number of entries to transform</param>
        virtual protected void TransformColumn(ref float[,] array, int column, int start, int count)
        {
            //will be overwritten by method of child class...
        }
        /// <summary>
        /// Performs a horizontal inverse transformation
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        /// <param name="row">index of the row which should be transformed</param>
        /// <param name="start">start position</param>
        /// <param name="count">number of entries to transform</param>
        virtual protected void BackTransformRow(ref float[,] array, int row, int start, int count)
        {
            //will be overwritten by method of child class...
        }
        /// <summary>
        /// Performs a vertical inverse transformation
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        /// <param name="column">index of the column which should be transformed</param>
        /// <param name="start">start position</param>
        /// <param name="count">number of entries to transform</param>
        virtual protected void BackTransformColumn(ref float[,] array, int column, int start, int count)
        {
            //will be overwritten by method of child class...
        }

        /*
        virtual protected void AfterTransformRows(ref float[,] array, int row, int start, int count)
        {
            //can be overwritten by method of child class...
        }
        virtual protected void AfterTransformColumns(ref float[,] array, int column, int start, int count)
        {
            //can be overwritten by method of child class...
        }
        virtual protected void AfterBackTransformRows(ref float[,] array, int row, int start, int count)
        {
            //can be overwritten by method of child class...
        }
        virtual protected void AfterBackTransformColumns(ref float[,] array, int column, int start, int count)
        {
            //can be overwritten by method of child class...
        }
        */
        #endregion
        #region public constructors
        /// <summary>
        /// Initalizes a two dimensional wavelet transformation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public WaveletPacket2D(int MinHorizontalSize, int MinVerticalSize, int AllowedMinSize, int PowerOf2Width, int PowerOf2Height, bool Scale)
        {
            Init(MinHorizontalSize, MinVerticalSize, AllowedMinSize, PowerOf2Width, PowerOf2Height, Scale);
        }

        /// <summary>
        /// Initalizes a two dimensional wavelet transformation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public WaveletPacket2D(int MinSize, int AllowedMinSize, int PowerOf2Width, int PowerOf2Height, bool Scale)
        {
            Init(MinSize, MinSize, AllowedMinSize, PowerOf2Width, PowerOf2Height, Scale);
        }
        #endregion
        #region public properties
        /// <summary>
        /// returns the width for the two dimensional wavelet transformation
        /// </summary>
        public int Width
        {
            get
            {
                return iWidth;
            }
        }
        /// <summary>
        /// returns the height for the two dimensional wavelet transformation
        /// </summary>
        public int Height
        {
            get
            {
                return iHeight;
            }
        }
        /// <summary>
        /// gets or sets the minimum size for horizontal transformation. This must be a power of two.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public int MinHorzizontalSize
        {
            get
            {
                return szMinHorz;
            }
            set
            {
                CheckPowerOf2("MinHorzizontalSize", value);
                if (value > iWidth)
                {
                    throw new ArgumentException("MinHorzizontalSize cannot be greater than " + iWidth);
                }
                if (value < iAllowedMinSize)
                {
                    throw new ArgumentException("MinVerticalSize cannot be smaller than " + iAllowedMinSize);
                }
                //All checks are done so we can set the new value
                szMinHorz = value;
            }
        }
        /// <summary>
        /// gets or sets the minimum size for vertical transformation. This must be a power of two.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public int MinVerticalSize
        {
            get
            {
                return szMinVert;
            }
            set
            {
                CheckPowerOf2("MinVerticalSize", value);
                if (value > iHeight)
                {
                    throw new ArgumentException("MinVerticalSize cannot be greater than " + iHeight);
                }
                if (value < iAllowedMinSize)
                {
                    throw new ArgumentException("MinVerticalSize cannot be smaller than " + iAllowedMinSize);
                }
                //All checks are done so we can set the new value
                szMinVert = value;
            }
        }
        #endregion
        #region public methods
        /// <summary>
        /// Perfroms a two dimensional separable wavelet transformation for an array. The result is copied back to the declared array.
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        virtual public void TransformSeparable2D(ref float[,] array)
        {
            int iSize;
            //transform each row
            iSize = iWidth;
            while (iSize >= szMinHorz)
            {
                //transform horizontal
                for (int y = 0; y < iHeight; y++)
                {
                    TransformRow(ref array, y, 0, iSize);
                }
                iSize = iSize >> 1;
            }

            //transform each column
            iSize = iHeight;
            while (iSize >= szMinVert)
            {
                //transform vertical
                for (int x = 0; x < iWidth; x++)
                {
                    TransformColumn(ref array, x, 0, iSize);
                }
                iSize = iSize >> 1;
            }
        }
        /// <summary>
        /// Perfroms a inverse two dimensional separable wavelet transformation for an array. The result is copied back to the declared array.
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        virtual public void BackTransformSeparable2D(ref float[,] array)
        {
            int iSize;

            iSize = szMinVert;
            while (iSize <= iHeight)
            {
                for (int x = 0; x < iWidth; x++)
                {
                    // back transform vertical
                    BackTransformColumn(ref array, x, 0, iSize);
                }
                iSize = iSize << 1;
            }
            iSize = szMinHorz;
            while (iSize <= iWidth)
            {
                for (int y = 0; y < iHeight; y++)
                {
                    // back transform horizontal
                    BackTransformRow(ref array, y, 0, iSize);
                }
                iSize = iSize << 1;
            }
        }
        /// <summary>
        /// Perfroms a two dimensional isotropic wavelet transformation for the declared area of the array. The result is copied back to the declared array.
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        /// <param name="offsetX">x position of the area</param>
        /// <param name="offsetY">y position of the area</param>
        /// <param name="width">width of the area</param>
        /// <param name="height">height of the area</param>
        /// <exception cref="ArgumentException"></exception>
        virtual public void TransformIsotropic2D(ref float[,] array, int offsetX, int offsetY, int width, int height)
        {
            //There might be better/simpler solutions to solve this transformation... However, it's working ;-)
            int szHorizontal, szVertical;
            bool bHDone = false, bVDone = false;

            CheckArguments(offsetX, offsetY, width, height);
            szHorizontal = width;
            szVertical = height;
            while ((bHDone == false) || (bVDone == false))
            {
                if (bHDone == false)
                {
                    for (int y = offsetY; y < szVertical + offsetY; y++)
                    {
                        //transform horizontal
                        TransformRow(ref array, y, offsetX, szHorizontal);
                    }
                }
                if (bVDone == false)
                {
                    for (int x = offsetX; x < szHorizontal + offsetX; x++)
                    {
                        //transform vertical
                        TransformColumn(ref array, x, offsetY, szVertical);
                    }
                }

                szVertical >>= 1; // divide height by 2 
                szHorizontal >>= 1; // divide width by 2

                if (szVertical < szMinVert)
                {
                    szVertical = szMinVert;
                    bVDone = true;
                }
                if (szHorizontal < szMinHorz)
                {
                    szHorizontal = szMinHorz;
                    bHDone = true;
                }
            }
        }
        /// <summary>
        /// Perfroms a two dimensional isotropic wavelet transformation for an array. The result is copied back to the declared array.
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        virtual public void TransformIsotropic2D(ref float[,] array)
        {
            TransformIsotropic2D(ref array, 0, 0, iWidth, iHeight);
        }
        /// <summary>
        /// Perfroms a inverse two dimensional isotropic wavelet transformation for the declared area of the array. The result is copied back to the declared array.
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        /// <param name="offsetX">x position of the area</param>
        /// <param name="offsetY">y position of the area</param>
        /// <param name="width">width of the area</param>
        /// <param name="height">height of the area</param>
        /// <exception cref="ArgumentException"></exception>
        virtual public void BackTransformIsotropic2D(ref float[,] array, int offsetX, int offsetY, int width, int height)
        {
            //There might be better/simpler solutions to solve this transformation... However, it's working ;-)
            int szHorizontal, szVertical; //horizontal size(width) and vertical size(height)
            int iDifference = IntegerMath.ILog2(width) - IntegerMath.ILog2(height);
            szHorizontal = szMinHorz;
            szVertical = szMinVert;
            CheckArguments(offsetX, offsetY, width, height);

            if (iDifference > 0) // width is greater than height
            {
                for (int i = 0; i < iDifference; i++)
                {
                    for (int y = offsetY; y < szVertical + offsetY; y++)
                    {
                        //back transform horizontal
                        BackTransformRow(ref array, y, offsetX, szHorizontal);
                    }
                    szHorizontal <<= 1; // multiply width by 2 
                }
            }
            else if (iDifference < 0) // height is greater than width
            {
                for (int i = 0; i < -iDifference; i++)
                {
                    for (int x = offsetX; x < szHorizontal + offsetX; x++)
                    {
                        //back transform vertical
                        BackTransformColumn(ref array, x, offsetY, szVertical);
                    }
                    szVertical <<= 1; // multiply height by 2 
                }
            }

            while (szHorizontal <= width) //just necessary to check the width, as the number of remaining horizontal and vertical transformations are equal now. (szVertical <= iHeight) || (szHorizontal <= iWidth))
            {
                for (int x = offsetX; x < szHorizontal + offsetX; x++)
                {
                    //back transform vertical
                    BackTransformColumn(ref array, x, offsetY, szVertical);
                }
                for (int y = offsetY; y < szVertical + offsetY; y++)
                {
                    //back transform horizontal
                    BackTransformRow(ref array, y, offsetX, szHorizontal);
                }
                szVertical <<= 1;
                szHorizontal <<= 1;
            }
        }
        /// <summary>
        /// Perfroms a inverse two dimensional isotropic wavelet transformation for an array. The result is copied back to the declared array.
        /// </summary>
        /// <param name="array">2d float array on which the transformation should be performed</param>
        virtual public void BackTransformIsotropic2D(ref float[,] array)
        {
            BackTransformIsotropic2D(ref array, 0, 0, iWidth, iHeight);
        }
        #endregion
    }
}
