// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib.Generator
{
    public class SudokuGenerator : GeneratorBase
    {
        private Random random = new Random();

        public SudokuGenerator(int x, int y, GameOutputDelegate processor, int[][] savedBoxes = null)
            : base(x, y, processor, savedBoxes)
        {
            for (int j = 0; j < Size; j++)
            {
                // Fill the first row of boxes
                boxes[0][j] = j + 1;
            }
        }

        /// <summary>
        /// Create an array of boolean which indicates open boxes of the game board
        /// </summary>
        /// <param name="numberOfOpenBoxes">number of open boxes</param>
        /// <returns>the mask</returns>
        public bool[][] CreateRandomMask(int numberOfOpenBoxes)
        {
            bool[][] mask = CreateArray(Size, Size, false);

            // Find numberOfOpenBoxes items to put true which means those boxes will be opened
            for (int i = 0; i < numberOfOpenBoxes; i++)
            {
                // Try to get a random box that is not opened 3 times
                int row, col;
                int time = 3;
                do
                {
                    row = random.Next(Size);
                    col = random.Next(Size);
                } while (mask[row][col] && --time >= 0);

                // After trying 3 times if the random box is still opened then move to the next box
                // If the box is the last box then move to the first box which has row of 0 and col of 0
                while (mask[row][col])
                {
                    if (IsLastBox(row, col))
                    {
                        row = col = 0;
                    }
                    else
                    {
                        (row, col) = GetNextBox(row, col);
                    }
                }

                mask[row][col] = true;
            }

            return mask;
        }

        /// <summary>
        /// Create a permutation of a list form 1 to Size
        /// </summary>
        /// <returns>the permutation</returns>
        public int[] GetPermutation()
        {
            var values = Enumerable.Range(1, Size).ToList();
            var permutation = new List<int>();
            for (int i = 0; i < Size; i++)
            {
                int randIndex = random.Next(values.Count());
                permutation.Add(values[randIndex]);
                values.RemoveAt(randIndex);
            }

            return permutation.ToArray();
        }

        /// <summary>
        /// Generate game boards
        /// </summary>
        /// <param name="continueFromLastStop">
        /// true: continue generating from the last stop
        /// false: generate from start
        /// </param>
        public void Generate(bool continueFromLastStop = false)
        {
            isStopped = false;
            Solve(1, 0, continueFromLastStop);
        }
    }
}
