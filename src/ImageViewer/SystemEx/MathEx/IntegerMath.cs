//Date: 2009-10-11
//Author: smoebius
//Description: Extended integer math functions
//LICENCE:
//This source is provided "as-is," without any express or implied warranty.
//The author is not liable for any damage caused by this software.
//Licenced under MIT licence
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemEx.MathEx
{
    /// <summary>
    /// Extended integer math functions
    /// </summary>
    public static class IntegerMath
    {
        private static Random rand = new Random();

        /// <summary>
        /// Calculates the greatest common divisor(GCD) of two numbers with the algorith of Euklid
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">second number</param>
        /// <returns>greatest common divisor(GCD)</returns>
        public static int CalcGCD(int a, int b)
        {
            int r;
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b > 0) // Algorith of Euklid
            {
                r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        /// <summary>
        /// return whether the value is a power of 2.
        /// </summary>
        /// <param name="value">unsigned integer</param>
        /// <returns>true if the value is a power of 2</returns>
        public static bool IsPowerOf2(uint value)
        {
            //Optimized for execution speed...
            if (value < 65536)
            {
                if (value < 256)
                {
                    //if (value == 0) { return true; } no power of 2...
                    if (value == 1) { return true; }
                    if (value == 2) { return true; }
                    if (value == 4) { return true; }
                    if (value == 8) { return true; }
                    if (value == 16) { return true; }
                    if (value == 32) { return true; }
                    if (value == 64) { return true; }
                    if (value == 128) { return true; }
                }
                else
                {
                    if (value == 256) { return true; }
                    if (value == 512) { return true; }
                    if (value == 1024) { return true; }
                    if (value == 2048) { return true; }
                    if (value == 4096) { return true; }
                    if (value == 8192) { return true; }
                    if (value == 16384) { return true; }
                    if (value == 32768) { return true; }
                }
            }
            else
            {
                if (value < 16777216)
                {
                    if (value == 65536) { return true; }
                    if (value == 131072) { return true; }
                    if (value == 262144) { return true; }
                    if (value == 524288) { return true; }
                    if (value == 1048576) { return true; }
                    if (value == 2097152) { return true; }
                    if (value == 4194304) { return true; }
                    if (value == 8388608) { return true; }
                }
                else
                {
                    if (value == 16777216) { return true; }
                    if (value == 33554432) { return true; }
                    if (value == 67108864) { return true; }
                    if (value == 134217728) { return true; }
                    if (value == 268435456) { return true; }
                    if (value == 536870912) { return true; }
                    if (value == 1073741824) { return true; }
                    if (value == 2147483648) { return true; }
                    //if (value == 4294967296) { return true; }            
                }
            }
            return false;
        }

        /// <summary>
        /// return whether the value is a power of 2.
        /// </summary>
        /// <param name="value">signed integer</param>
        /// <returns>true if the value is a power of 2</returns>
        public static bool IsPowerOf2(int value)
        {
            return IsPowerOf2((uint)value);
        }

        /// <summary>
        /// Returns the exponent for a power of 2 value. If the value is no power of 2 it returns -1.
        /// </summary>
        /// <param name="value">unsigned integer</param>
        /// <returns></returns>
        public static int ILog2(uint value)
        {
            // slow and ugly way: 
            //(int)(Math.Log(width) / Math.Log(2) + 0.5d);

            if (value < 65536)
            {
                if (value < 256)
                {
                    if (value == 1) { return 0; }
                    if (value == 2) { return 1; }
                    if (value == 4) { return 2; }
                    if (value == 8) { return 3; }
                    if (value == 16) { return 4; }
                    if (value == 32) { return 5; }
                    if (value == 64) { return 6; }
                    if (value == 128) { return 7; }
                }
                else
                {
                    if (value == 256) { return 8; }
                    if (value == 512) { return 9; }
                    if (value == 1024) { return 10; }
                    if (value == 2048) { return 11; }
                    if (value == 4096) { return 12; }
                    if (value == 8192) { return 13; }
                    if (value == 16384) { return 14; }
                    if (value == 32768) { return 15; }
                }
            }
            else
            {
                if (value < 16777216)
                {
                    if (value == 65536) { return 16; }
                    if (value == 131072) { return 17; }
                    if (value == 262144) { return 18; }
                    if (value == 524288) { return 19; }
                    if (value == 1048576) { return 20; }
                    if (value == 2097152) { return 21; }
                    if (value == 4194304) { return 22; }
                    if (value == 8388608) { return 23; }
                }
                else
                {
                    if (value == 16777216) { return 24; }
                    if (value == 33554432) { return 25; }
                    if (value == 67108864) { return 26; }
                    if (value == 134217728) { return 27; }
                    if (value == 268435456) { return 28; }
                    if (value == 536870912) { return 29; }
                    if (value == 1073741824) { return 30; }
                    if (value == 2147483648) { return 31; }
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the exponent for a power of 2 value. If the value is no power of 2 it returns -1.
        /// </summary>
        /// <param name="value">signed integer</param>
        /// <returns></returns>
        public static int ILog2(int value)
        {
            return ILog2((uint)value);
        }


        /// <summary>
        /// Makes sure the value is a power of 2. e.g. if value is 77 it will return 128.
        /// </summary>
        /// <param name="value">unsigned integer</param>
        /// <returns></returns>
        public static uint ToPowerOf2(uint value)
        {
            /* Old function
            private int ToPowerOf2(int iValue)
            {
                int iPower = (int)(Math.Log((double)iValue) / Math.Log(2)) + 1;
                return (int)Math.Pow(2, (double)iPower);
            }
            */
            if (IsPowerOf2(value))
            {
                return value;
            }
            else
            {
                // do not use this for power of 2 numbers...
                uint newValue = 1;
                while (value > 0)
                {
                    value = value >> 1;
                    newValue = newValue << 1;
                }
                return newValue;
            }
        }

        /// <summary>
        /// Makes sure the value is a power of 2. e.g. if value is 77 it will return 128.
        /// </summary>
        /// <param name="value">signed integer</param>
        /// <returns></returns>
        public static int ToPowerOf2(int value)
        {
            return (int)ToPowerOf2((uint)value);
        }

        /// <summary>
        /// Returns 2^power. 
        /// </summary>
        /// <param name="power">unsigned integer</param>
        /// <returns></returns>
        public static uint IPow2(uint power)
        {
            return (uint)(1 << (int)power);
        }

        /// <summary>
        /// Returns 2^power. 
        /// </summary>
        /// <param name="power">signed integer</param>
        /// <returns></returns>
        public static int IPow2(int power)
        {
            return 1 << power;
        }

        /// <summary>
        /// Returns whether the value is odd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Odd(int value)
        {
            return ((value & 1) == 1) ? true : false;
        }

        /// <summary>
        /// Calculates the faculty for the declared value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Fac(int value)
        {
            //approximation formula: Sqr(2 * PI * n) * Pow(n/e,n)
            //throw new OverflowException("cannot calculate faculty of " + value.ToString());
            if (value < 0)
            {
                throw new ArgumentException("value cannot be be negative");
            }
            int result = 1;
            for (int i = value; i > 1; i--)
            {
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// Calculates the faculty for the declared value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static long Fac(long value)
        {
            //approximation formula: Sqr(2 * PI * n) * Pow(n/e,n)
            //throw new OverflowException("cannot calculate faculty of " + value.ToString());
            if (value < 0)
            {
                throw new ArgumentException("value cannot be be negative");
            }
            long result = 1;
            for (long i = value; i > 1; i--)
            {
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// Changes the start value for the random numbers returned by Random()
        /// </summary>
        static public void RandomSeed(int iSeed)
        {
            rand = new Random(iSeed);
        }

        /// <summary>
        /// Returns a random number between 0 and iMax (icluding 0 and iMax)
        /// </summary>
        static public int Random(int iMax)
        {
            if (iMax < 0)
            {
                return -rand.Next(-iMax + 1);
            }
            else
            {
                return rand.Next(iMax + 1);
            }
        }


        /// <summary>
        /// Calculates the entropy of an integer array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double CalculateEntropy(int[] array)
        {
            int iMax = int.MinValue;
            int iMin = int.MaxValue;
            int iValue;
            int iLength;
            int iArrayLength = array.Length;
            double dP, dEntropy = 0.0d;
            int[] symbols;

            if (iArrayLength > 0)
            {
                for (int i = 0; i < iArrayLength; i++)
                {
                    iValue = array[i];
                    if (iValue < iMin)
                    {
                        iMin = iValue;
                    }
                    if (iValue > iMax)
                    {
                        iMax = iValue;
                    }
                }
                iLength = iMax - iMin;
                symbols = new int[iLength+1];
                for (int i = 0; i < iArrayLength; i++)
                {
                    symbols[array[i] - iMin]++;
                }

                for (int i = 0; i < symbols.Length; i++)
                {
                    if (symbols[i] > 0)
                    {
                        
                        dP = (double)symbols[i] / ((double)iArrayLength); // Propability for the symbol
                        dEntropy += dP * Math.Log(dP) / Math.Log(2); // Formula described in book "Bilddatenkompression" on page 7
                    }
                }
                dEntropy = -dEntropy;
            }
            return dEntropy;
        }



        //Eval
    }
}
