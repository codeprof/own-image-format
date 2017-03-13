//Date: 2009-10-11
//Author: smoebius
//Description: Extended float point math functions
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//It is for educational purpose only. Therefore it is not allowed to use the whole source or parts of it in other software products.
//Any redistribution is prohibited without the explicit permisson of the author (Stefan Moebius).
//The author is not liable for any damage caused by this software.
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemEx.MathEx
{
    /// <summary>
    /// Extended float point math functions
    /// </summary>
    public static class FloatpointMath
    {
        /// <summary>
        /// Determines if the declared floating-point value is not a number (e.g. when dividing by zero)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotANumber(double value)
        {
            return double.IsNaN(value);
        }

        /// <summary>
        /// Determines if the declared floating-point value is not a number (e.g. when dividing by zero)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotANumber(float value)
        {
            return float.IsNaN(value);
        }

        /// <summary>
        /// Determines if the declared floating-point value is finite
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFinite(double value)
        {
            return !double.IsInfinity(value);
        }

        /// <summary>
        /// Determines if the declared floating-point value is finite
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFinite(float value)
        {
            return !float.IsInfinity(value);
        }

        /// <summary>
        /// Performs a linear interpolation based on the formula "a + s * (b - a)" 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double Lerp(double a, double b, double s)
        {
        
            return a + s * (b - a);
        }

        /// <summary>
        /// Performs a linear interpolation based on the formula "a + s * (b - a)" 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float s)
        {

            return a + s * (b - a);
        }   

        /// <summary>
        /// Clamps the declared value within the range of 0.0 to 1.0
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Saturate(double a)
        {
            if (a > 1.0d) { return 1.0d; }
            if (a < 0.0d) { return 0.0d; }
            return a;
        }

        /// <summary>
        /// Clamps the declared value within the range of 0.0 to 1.0
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float Saturate(float a)
        {
            if (a > 1.0d) { return 1.0f; }
            if (a < 0.0d) { return 0.0f; }
            return a;
        }

        /// <summary>
        /// Clamps the specified value to the declared minimum and maximum range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(double value, double min, double max)
        {
            if (value > max) { return max; }
            if (value < min) { return min; }
            return value;
        }

        /// <summary>
        /// Clamps the specified value to the declared minimum and maximum range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value > max) { return max; }
            if (value < min) { return min; }
            return value;
        }

        /// <summary>
        /// Converts the declared value from degree to radian
        /// </summary>
        /// <param name="degree">angle in degree</param>
        /// <returns>angle in radian</returns>
        public static double Radian(double degree)
        {
            return degree * 2.0d * Math.PI / 360.0d; 
        }

        /// <summary>
        /// Converts the declared value from degree to radian
        /// </summary>
        /// <param name="degree">angle in degree</param>
        /// <returns>angle in radian</returns>
        public static float Radian(float degree)
        {
            return degree * 2.0f * (float)Math.PI / 360.0f;
        }

        /// <summary>
        /// Converts the declared value from radian to degree
        /// </summary>
        /// <param name="radian">angle in radian</param>
        /// <returns>angle in degree</returns>
        public static double Degree(double radian)
        {
            return radian * 360.0d / (2.0d * Math.PI);
        }

        /// <summary>
        /// Converts the declared value from radian to degree
        /// </summary>
        /// <param name="radian">angle in radian</param>
        /// <returns>angle in degre</returns>
        public static float Degree(float radian)
        {
            return radian * 360.0f / (2.0f * (float)Math.PI);
        }

        /// <summary>
        /// Performs a linear regression.
        /// </summary>
        /// <param name="src">array of Y positions</param>
        /// <param name="num">number of positions</param>
        /// <param name="out_y">intercept of the linear regression</param>
        /// <param name="out_m">slope of the linear regression</param>
        public static void LinearRegression(ref double[] src, int num, ref double out_y, ref double out_m)
        {
            double dy = 0.0, dx = 0.0, sumsxy = 0.0, sxy = 0.0, sx = 0.0, sy = 0.0, rxy = 0.0;
            for (int T = 0; T < num; T++)
            {
                dy += src[T];
            }
            dy = dy /((double)num);

            dx = ((double)num-1) /2;

            for (int T=0; T < num; T++)
            {
                sumsxy += ((double)T - dx) * (src[T] - dy);
            }
            sxy=1.0d/( (double)num - 1.0d) * sumsxy;

            for (int T=0; T <num; T++)
            {
                sx += Math.Pow((double)T - dx, 2);
            }
            sx =Math.Sqrt(1.0d / (num - 1.0d) * sx);

            for (int T=0; T < num; T++)
            {
                sy += Math.Pow(src[T] - dy, 2);
            }
            sy = Math.Sqrt(1.0d / ((double)num - 1.0d) * sy);

            rxy = sxy / (sx * sy);

            out_y = rxy* sy/sx;
            out_m = dy - out_y * dx;
        }

        /// <summary>
        /// Performs a linear regression.
        /// </summary>
        /// <param name="src">array of Y positions</param>
        /// <param name="num">number of positions</param>
        /// <param name="out_y">intercept of the linear regression</param>
        /// <param name="out_m">slope of the linear regression</param>
        public static void LinearRegression(ref float[] src, int num, ref float out_y, ref float out_m)
        {
            float dy = 0.0f, dx = 0.0f, sumsxy = 0.0f, sxy = 0.0f, sx = 0.0f, sy = 0.0f, rxy = 0.0f;
            for (int T = 0; T < num; T++)
            {
                dy += src[T];
            }
            dy = dy / ((float)num);

            dx = ((float)num - 1) / 2;

            for (int T = 0; T < num; T++)
            {
                sumsxy += ((float)T - dx) * (src[T] - dy);
            }
            sxy = 1.0f / ((float)num - 1.0f) * sumsxy;

            for (int T = 0; T < num; T++)
            {
                sx += (float)Math.Pow((float)T - dx, 2);
            }
            sx = (float)Math.Sqrt(1.0f / (num - 1.0d) * sx);

            for (int T = 0; T < num; T++)
            {
                sy += (float)Math.Pow(src[T] - dy, 2);
            }
            sy = (float)Math.Sqrt(1.0f / ((float)num - 1.0f) * sy);

            rxy = sxy / (sx * sy);

            out_y = rxy * sy / sx;
            out_m = dy - out_y * dx;
        }

        /// <summary>
        /// Performs a linear regression.
        /// </summary>
        /// <param name="src">array of Y positions</param>
        /// <param name="out_y">intercept of the linear regression</param>
        /// <param name="out_m">slope of the linear regression</param>
        public static void LinearRegression(ref double[] src, ref double out_y, ref double out_m)
        {
            LinearRegression(ref src, src.Length, ref out_y, ref out_m);
        }

        /// <summary>
        /// Performs a linear regression.
        /// </summary>
        /// <param name="src">array of Y positions</param>
        /// <param name="out_y">intercept of the linear regression</param>
        /// <param name="out_m">slope of the linear regression</param>
        public static void LinearRegression(ref float[] src, ref float out_y, ref float out_m)
        {
            LinearRegression(ref src, src.Length, ref out_y, ref out_m);
        }

        /// <summary>
        /// Calculates the one-dimensional distance
        /// </summary>
        /// <param name="x1">position of the first point</param>
        /// <param name="x2">position of the second point</param>
        /// <returns>one-dimensional distance</returns>
        public static double Distance1D(double x1,double x2)
        {
            //return Math.Abs(x2 - x1);
            if (x1 >= x2)
            {
                return x1 - x2;
            }
            else
            {
                return x2 - x1;
            }
        }

        /// <summary>
        /// Calculates the one-dimensional distance
        /// </summary>
        /// <param name="x1">position of the first point</param>
        /// <param name="x2">position of the second point</param>
        /// <returns>one-dimensional distance</returns>
        public static float Distance1D(float x1, float x2)
        {
            //return Math.Abs(x2 - x1);
            if (x1 >= x2)
            {
                return x1 - x2;
            }
            else
            {
                return x2 - x1;
            }
        }

        /// <summary>
        /// Calculates distance between the two two-dimensional points
        /// </summary>
        /// <param name="x1">x-position of the first point</param>
        /// <param name="y1">y-position of the first point</param>
        /// <param name="x2">x-position of the second point</param>
        /// <param name="y2">y-position of the second point</param>
        /// <returns>distance between the two two-dimensional points</returns>
        public static double Distance2D(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates distance between the two two-dimensional points
        /// </summary>
        /// <param name="x1">x-position of the first point</param>
        /// <param name="y1">y-position of the first point</param>
        /// <param name="x2">x-position of the second point</param>
        /// <param name="y2">y-position of the second point</param>
        /// <returns>distance between the two two-dimensional points</returns>
        public static float Distance2D(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates distance between the two three-dimensional points
        /// </summary>
        /// <param name="x1">x-position of the first point</param>
        /// <param name="y1">y-position of the first point</param>
        /// <param name="z1">z-position of the first point</param>
        /// <param name="x2">x-position of the second point</param>
        /// <param name="y2">y-position of the second point</param>
        /// <param name="z2">z-position of the second point</param>
        /// <returns>distance between the two three-dimensional points</returns>
        public static double Distance3D(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates distance between the two three-dimensional points
        /// </summary>
        /// <param name="x1">x-position of the first point</param>
        /// <param name="y1">y-position of the first point</param>
        /// <param name="z1">z-position of the first point</param>
        /// <param name="x2">x-position of the second point</param>
        /// <param name="y2">y-position of the second point</param>
        /// <param name="z2">z-position of the second point</param>
        /// <returns>distance between the two three-dimensional points</returns>
        public static float Distance3D(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float dz = z2 - z1;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the average of the two values
        /// </summary>
        /// <param name="a">1. double value</param>
        /// <param name="b">2. double value</param>
        /// <returns>return the average of the two values</returns>
        public static double Avg(double a, double b)
        {
            return  0.5d * (a + b);
        }

        /// <summary>
        /// Calculates the average of the two values
        /// </summary>
        /// <param name="a">1. float value</param>
        /// <param name="b">2. float value</param>
        /// <returns>return the average of the two values</returns>
        public static float Avg(float a, float b)
        {
            return 0.5f * (a + b);
        }

        /// <summary>
        /// Calculates the average of the three values
        /// </summary>
        /// <param name="a">1. double value</param>
        /// <param name="b">2. double value</param>
        /// <param name="c">3. double value</param>
        /// <returns>return the average of the three values</returns>
        public static double Avg(double a, double b,double c)
        {
            return 0.3333333333333333333d * (a + b + c);
        }

        /// <summary>
        /// Calculates the average of the three values
        /// </summary>
        /// <param name="a">1. float value</param>
        /// <param name="b">2. float value</param>
        /// <param name="c">3. float value</param>
        /// <returns>return the average of the three values</returns>
        public static float Avg(float a, float b, float c)
        {
            return 0.3333333333333333333f * (a + b + c);
        }

        /// <summary>
        /// Calculates the average of the four values
        /// </summary>
        /// <param name="a">1. double value</param>
        /// <param name="b">2. double value</param>
        /// <param name="c">3. double value</param>
        /// <param name="d">4. double value</param>
        /// <returns>return the average of the four values</returns>
        public static double Avg(double a, double b, double c, double d)
        {
            return 0.25d * (a + b + c + d);
        }

        /// <summary>
        /// Calculates the average of the four values
        /// </summary>
        /// <param name="a">1. float value</param>
        /// <param name="b">2. float value</param>
        /// <param name="c">3. float value</param>
        /// <param name="d">4. float value</param>
        /// <returns>return the average of the four values</returns>
        public static float Avg(float a, float b, float c, float d)
        {
            return 0.25f * (a + b + c + d);
        }

        /// <summary>
        /// Calculates the average of the five values
        /// </summary>
        /// <param name="a">1. double value</param>
        /// <param name="b">2. double value</param>
        /// <param name="c">3. double value</param>
        /// <param name="d">4. double value</param>
        /// <param name="e">5. double value</param>
        /// <returns>return the average of the five values</returns>
        public static double Avg(double a, double b, double c, double d, double e)
        {
            return 0.2d * (a + b + c + d + e);
        }

        /// <summary>
        /// Calculates the average of the five values
        /// </summary>
        /// <param name="a">1. float value</param>
        /// <param name="b">2. float value</param>
        /// <param name="c">3. float value</param>
        /// <param name="d">4. float value</param>
        /// <param name="e">5. float value</param>
        /// <returns>return the average of the five values</returns>
        public static float Avg(float a, float b, float c, float d, float e)
        {
            return 0.2f * (a + b + c + d + e);
        }

        /// <summary>
        /// returns the sinc function of x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Sinc(double x)
        {
            return x != 0.0 ? Math.Sin(x) / x : 1.0; // avoids dividing by zero
        }

        /// <summary>
        /// returns the sinc function of x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Sinc(float x)
        {
            return x != 0.0f ? (float)Math.Sin(x) / x : 1.0f; // avoids dividing by zero
        }
    }
}
