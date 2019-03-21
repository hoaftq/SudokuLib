using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuLib.Generator;

namespace SudokuLibTest.Generator
{
    [TestClass]
    public class SodokuSolverTest
    {
        [TestMethod]
        public void SolverTest1()
        {
            int X = 3, Y = 3;
            int[][] presetBoard =
            {
                new int[]{4, 0, 0,   2, 0, 6,   1, 7, 0},
                new int[]{0, 9, 0,   5, 0, 3,   0, 0, 0},
                new int[]{0, 5, 0,   0, 7, 0,   6, 9, 0},

                new int[]{0, 0, 0,   7, 3, 0,   0, 1, 0},
                new int[]{0, 0, 0,   0, 5, 0,   2, 0, 9},
                new int[]{0, 0, 0,   0, 4, 1,   0, 0, 6},

                new int[]{0, 4, 5,   0, 6, 0,   0, 0, 8},
                new int[]{0, 7, 8,   4, 0, 0,   5, 0, 0},
                new int[]{0, 0, 6,   3, 0, 5,   0, 0, 0}
            };
            var solver = new SudokuSolver(X, Y, Processor);
            solver.InitializeBoard(presetBoard);
            solver.Solve();

            bool Processor(int[][] contents)
            {
                TestUtils.Print(contents);
                TestUtils.VerifyResult(X, Y, contents);

                // Finding all the results
                return true;
            }
        }

        [TestMethod]
        public void SolverTest2()
        {
            int X = 3, Y = 3;
            int numberOfResults = 0;
            int[][] presetBoard =
            {
                new int[]{4, 0, 0,   2, 0, 6,   1, 7, 0},
                new int[]{0, 9, 0,   5, 0, 3,   0, 0, 0},
                new int[]{0, 5, 0,   0, 7, 0,   6, 9, 0},

                new int[]{0, 0, 0,   7, 3, 0,   0, 1, 0},
                new int[]{0, 0, 0,   0, 5, 0,   2, 0, 9},
                new int[]{0, 0, 0,   0, 4, 1,   0, 0, 6},

                new int[]{0, 4, 5,   0, 6, 0,   0, 0, 8},
                new int[]{0, 7, 8,   4, 0, 0,   5, 0, 0},
                new int[]{0, 0, 0,   0, 0, 0,   0, 0, 0}
            };
            var solver = new SudokuSolver(X, Y, Processor);
            solver.InitializeBoard(presetBoard);
            solver.Solve();

            // Expected getting more than just 1 result
            Assert.IsTrue(numberOfResults > 1);

            bool Processor(int[][] contents)
            {
                TestUtils.Print(contents);
                TestUtils.VerifyResult(X, Y, contents);

                numberOfResults++;

                // Finding all the results
                return true;
            }
        }
    }
}
