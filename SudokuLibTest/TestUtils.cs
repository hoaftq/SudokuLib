using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLibTest
{
    public static class TestUtils
    {
        public static void Print<T>(T[][] contents)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                for (int j = 0; j < contents[0].Length; j++)
                {
                    Console.Write($"{contents[i][j]}  ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("------------------------------");
        }

        public static void VerifyResult(int x, int y, int[][] contents)
        {
            int size = x * y;
            for (int i = 0; i < size; i++)
            {
                // Verify row
                Assert.IsTrue(IsValidRange(contents[i]), $"Row {i} is invalid");

                // Verify column
                Assert.IsTrue(IsValidRange(GetColumn(i)), $"Column {i} is invalid");
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    // Verify square
                    Assert.IsTrue(IsValidRange(GetSquare(i, j)), $"Square {i}x{j} is invalid");
                }
            }

            IEnumerable<int> GetColumn(int col)
            {
                for (int i = 0; i < size; i++)
                {
                    yield return contents[i][col];
                }
            }

            IEnumerable<int> GetSquare(int r, int c)
            {
                for (int i = r * y; i < (r + 1) * y; i++)
                {
                    for (int j = c * x; j < (c + 1) * x; j++)
                    {
                        yield return contents[i][j];
                    }
                }
            }
        }

        /// <summary>
        /// A valid range must has the value from 1 to <paramref name="range"/>.size
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        private static bool IsValidRange(IEnumerable<int> range)
        {
            var values = range.OrderBy(r => r).ToList();
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] != i + 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
