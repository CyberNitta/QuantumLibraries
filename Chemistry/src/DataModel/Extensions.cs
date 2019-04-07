﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Quantum.Chemistry
{
    public static partial class Extensions
    {

        #region Extension methods
        /// <summary>
        /// Computes the L_p norm of coefficicients of all terms in a Hamiltonian.
        /// </summary>
        /// <param name="power">Selects type of norm.</param>
        /// <returns>L_p norm of Hamiltonian coefficients.</returns>
        public static double Norm(this IEnumerable<double> x, double power = 1.0)
        {
            double sum = x.Select(y => Math.Pow(Math.Abs(y), power)).Sum();
            return Math.Pow(sum, 1.0 / power);
        }

        /// <summary>
        /// Construct Double that implements the ITermValue interface. 
        /// </summary>
        /// <param name="x">Input double.</param>
        /// <returns>Double representing the input double.</returns>
        public static Double ToDouble(this double x)
        {
            return new Double(x);
        }

        /// <summary>
        /// Computes `x^y` for an integer base `x` and exponent `y`
        /// </summary>
        /// <param name="x">Base.</param>
        /// <param name="exponent">Exponent.</param>
        /// <returns>An integer `x^y`</returns>
        public static int Pow(this int x, int exponent)
        {
            int output = 1;
            for (int i = 0; i < exponent; i++)
            {
                output *= x;
            }
            return output;
        }

        /// <summary>
        /// String representation of elements of an <see cref="IEnumerable{T}"/> of <see cref="int"/>.
        /// </summary>
        /// <param name="ints"><see cref="IEnumerable{T}"/> of <see cref="int"/>.</param>
        /// <returns>String representation of input elements.</returns>
        public static string Print(this IEnumerable<int> ints)
        {
            return "[" + string.Join(", ", ints) + "]";
        }


        /// <summary>
        /// Checks whether a sequence of integers {x,y,z,...} is sorted in
        /// ascending order x <= y <= z <= ...
        /// </summary>
        /// <param name="x">Sequence of integers to be checked.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="x"/> is sorted in ascending
        /// order, and <c>false</c> otherwise.
        /// </returns>
        public static bool IsIntArrayAscending(this IEnumerable<long> x)
        {
            // return true for empty list.
            if (!x.Any())
            {
                return true;
            }
            var curr = x.First();
            foreach (var next in x.Skip(1))
            {
                if (next < curr)
                {
                    return false;
                }
                curr = next;
            }
            return true;
        }
        public static bool IsIntArrayAscending(this IEnumerable<int> x)
        {
            return x.Select(o => (long)o).IsIntArrayAscending();
        }
        #endregion


        #region Int64 Comparers


        /// <summary>
        /// Compares two equal-length sequences of integers by perform an element-wise
        /// comparison, starting from the first element.
        /// </summary>
        /// <param name="xArr">First sequence of integers.</param>
        /// <param name="yArr">Second sequence of integers.</param>
        /// <returns>
        /// Returns <c>1</c> if <paramref name="xArr"/> is greater than <paramref name="yArr"/>.
        /// Returns <c>-1</c> if <paramref name="xArr"/> is less than <paramref name="yArr"/>.
        /// Returns <c>0</c> if <paramref name="xArr"/> is equal to <paramref name="yArr"/>.
        /// </returns>
        /// <example>
        /// CompareIntArray(new Int64[] {5}, new Int64[] {7}) == -1;
        /// CompareIntArray(new Int64[] {5,7}, new Int64[] {5,6}) == 1;
        /// CompareIntArray(new Int64[] {2,1,3}, new Int64[] {2,2,3}) == -1;
        /// </example>
        public static int CompareIntArray(IEnumerable<Int64> xArr, IEnumerable<Int64> yArr)
        {
            foreach (var item in xArr.Zip(yArr, (x, y) => (x, y)))
            {
                if (item.y > item.x)
                {
                    return -1;
                }
                else if (item.y < item.x)
                {
                    return 1;
                }
            }
            return 0;
        }


        /// <summary>
        /// IComparer for two integers.
        /// </summary>
        public class Int64IComparer : IComparer<Int64>
        {
            public int Compare(Int64 x, Int64 y) => Math.Sign(x - y);
        }


        public class Int64ArrayIComparer : IComparer<IEnumerable<Int64>>
        {
            public int Compare(IEnumerable<Int64> x, IEnumerable<Int64> y)
            {
                return CompareIntArray(x, y);
            }
        }

        public class Int64ArrayIEqualityComparer : IEqualityComparer<IEnumerable<Int64>>
        {
            public bool Equals(IEnumerable<Int64> x, IEnumerable<Int64> y)
            {
                return x.SequenceEqual(y);
            }
            public int GetHashCode(IEnumerable<Int64> x)
            {
                int h = 19;
                foreach (var i in x)
                {
                    h = h * 31 + ((int)i);
                }
                return h;
            }
        }
        #endregion

        #region int Comparers
        public class IntArrayIEqualityComparer : IEqualityComparer<IEnumerable<int>>
        {
            public bool Equals(IEnumerable<int> x, IEnumerable<int> y)
            {
                return x.SequenceEqual(y);
            }
            public int GetHashCode(IEnumerable<int> x)
            {
                int h = 19;
                foreach (var i in x)
                {
                    h = h * 31 + ((int)i);
                }
                return h;
            }
        }

        public static int CompareIntArray(IEnumerable<int> xArr, IEnumerable<int> yArr)
        {
            foreach (var item in xArr.Zip(yArr, (x, y) => (x, y)))
            {
                if (item.y > item.x)
                {
                    return -1;
                }
                else if (item.y < item.x)
                {
                    return 1;
                }
            }
            return 0;
            
        }

        /// <summary>
        /// IComparer for two integers.
        /// </summary>
        public class IntIComparer : IComparer<int>
        {
            public int Compare(int x, int y) => Math.Sign(x - y);
        }


        public class IntArrayIComparer : IComparer<IEnumerable<int>>
        {
            public int Compare(IEnumerable<int> x, IEnumerable<int> y)
            {
                return CompareIntArray(x, y);
            }
        }
        #endregion

        #region Map
        /// <summary>
        ///      Given a value of an enumeration type, and an action for each
        ///      possible value of that enumeration type, performs the action
        ///      corresponding to the given value.
        /// </summary>
        public static void Map<E>
            (this E @enum, params (E, Action)[] actions)
            where E : struct, IConvertible
        {
            foreach (var (value, action) in actions)
            {
                if (@enum.Equals(value))
                {
                    action();
                    return;
                }
            }
        }

        /// <summary>
        ///      Given a value of an enumeration type, and an function for each
        ///      possible value of that enumeration type, returns the value
        //       returned by the function corresponding to the given value.
        /// </summary>
        public static T Map<E, T>
            (this E @enum, params (E, Func<T>)[] actions)
            where E : struct, IConvertible
        {
            foreach (var (value, action) in actions)
            {
                if (@enum.Equals(value))
                {
                    return action();
                }
            }

            throw new ArgumentException($"Expected {@enum} to be a member of {@enum.GetType()}.");
        }

        #endregion
    }
}
