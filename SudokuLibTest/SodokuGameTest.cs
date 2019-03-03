using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLibTest
{
    [TestClass]
    public class SodokuGameTest
    {
        [TestMethod]
        public void SudokuGameTest1()
        {
            int X = 3, Y = 2;
            var game = new SudokuGame(X, Y, Processor);
            game.Generate();

            bool Processor(int[][] contents)
            {
                Print(contents);
                VerifyResult(X, Y, contents);

                // Stop generating new game board
                return false;
            }
        }

        [TestMethod]
        public void SudokuGameTest2()
        {
            int X = 3, Y = 3;
            int numberOfResult = 0;
            var game = new SudokuGame(X, Y, Processor);
            game.Generate();

            bool Processor(int[][] contents)
            {
                numberOfResult++;
                Print(contents);
                VerifyResult(X, Y, contents);

                if (numberOfResult == 1000)
                    return false;

                return true;
            }
        }

        [TestMethod]
        public void SudokuGameTest3()
        {
            int numberOfResult = 0;
            int X = 3, Y = 3;
            int time = 1;

            var game = new SudokuGame(X, Y, Processor);

            game.Generate();

            Console.WriteLine("******************************");
            time = 2;
            game.Generate(true);

            bool Processor(int[][] contents)
            {
                VerifyResult(X, Y, contents);
                switch (time)
                {
                    // First generate call
                    case 1:
                        Print(contents);
                        if (++numberOfResult == 3)
                        {
                            // Stop generate 1
                            return false;
                        }
                        break;

                    // Second generate call
                    case 2:
                        Print(contents);
                        if (++numberOfResult >= 5)
                        {
                            return false;
                        }
                        break;
                }

                return true;
            }
        }


        private static void Print(int[][] contents)
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

        private void VerifyResult(int x, int y, int[][] contents)
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
        private bool IsValidRange(IEnumerable<int> range)
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
