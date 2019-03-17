using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuLib.Generator;
using System;
using System.Linq;

namespace SudokuLibTest.Generator
{
    [TestClass]
    public class SudokuGeneratorTest
    {
        [TestMethod]
        public void SudokuGameTest1()
        {
            int X = 3, Y = 2;
            var game = new SudokuGenerator(X, Y, Processor);
            game.Generate();

            bool Processor(int[][] contents)
            {
                TestUtils.Print(contents);
                TestUtils.VerifyResult(X, Y, contents);

                // Stop generating new game board
                return false;
            }
        }

        [TestMethod]
        public void SudokuGameTest2()
        {
            int X = 3, Y = 3;
            int numberOfResult = 0;
            var game = new SudokuGenerator(X, Y, Processor);
            game.Generate();

            bool Processor(int[][] contents)
            {
                numberOfResult++;
                TestUtils.Print(contents);
                TestUtils.VerifyResult(X, Y, contents);

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

            var game = new SudokuGenerator(X, Y, Processor);

            game.Generate();

            Console.WriteLine("******************************");
            time = 2;
            game.Generate(true);

            bool Processor(int[][] contents)
            {
                TestUtils.VerifyResult(X, Y, contents);
                switch (time)
                {
                    // First generate call
                    case 1:
                        TestUtils.Print(contents);
                        if (++numberOfResult == 3)
                        {
                            // Stop generate 1
                            return false;
                        }
                        break;

                    // Second generate call
                    case 2:
                        TestUtils.Print(contents);
                        if (++numberOfResult >= 5)
                        {
                            return false;
                        }
                        break;
                }

                return true;
            }
        }

        [TestMethod]
        public void CreateRandomMaskTest1()
        {
            int x = 3, y = 3,
                numberOfOpenBoxes = 25;
            var game = new SudokuGenerator(x, y, null);
            var mask = game.CreateRandomMask(numberOfOpenBoxes);
            int numberOfTrue = mask.Sum(r => r.Count(c => c));
            TestUtils.Print(mask);

            Assert.AreEqual(numberOfOpenBoxes, numberOfTrue);
        }

        [TestMethod]
        public void GetPermutationTest1()
        {
            int x = 3, y = 3;
            var game = new SudokuGenerator(x, y, null);
            int[] values = game.GetPermutation();
            for (int i = 1; i <= x * y; i++)
            {
                Assert.IsTrue(values.Contains(i), $"The permutation does not container {i}");
            }
        }
    }
}
