// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib
{
    public delegate bool GameOutputDelegate(int[][] contents);

    /// <summary>
    ///           -----col-----
    ///           --X--   --X--
    ///  |    Y   1 2 3   4 5 6   <- the first row is always initialized with the values from 1 to Size
    ///  |    |   x x x   x x x
    ///  |        
    ///  row  Y   x x x   x x x
    ///  |    |   x x x   x x x
    ///  |        
    ///  |    Y   x x x   x x x
    ///  |    |   x x x   x x x
    /// </summary>
    public class SudokuGenerator
    {
        private const int EMPTY = 0;

        private Random random = new Random();

        private int[][] boxes;

        private int[][] savedBoxes;

        private List<int> allCandidates = new List<int>();

        private GameOutputDelegate processor;

        // true means there is a request to stop the generator
        private bool isStopped;

        public int X { get; }

        public int Y { get; }

        public int Size { get; }

        /// <summary>
        /// A Sudoku game that can generate game boards
        /// </summary>
        /// <param name="x">Width of a block</param>
        /// <param name="y">Height of a block</param>
        /// <param name="processor">
        /// This will be executed when a solution is found.
        /// If the function returns false then the generating process will be stopped
        /// </param>
        /// <param name="savedBoxes">previous state that wanted to start generating from</param>
        public SudokuGenerator(int x, int y, GameOutputDelegate processor, int[][] savedBoxes = null)
        {
            X = x;
            Y = y;
            Size = X * Y;
            this.processor = processor ?? (r => false);
            this.savedBoxes = savedBoxes ?? CreateArray(Size, Size, 1);

            // Initialize boxes
            boxes = CreateArray(Size, Size, EMPTY);
            for (int j = 0; j < Size; j++)
            {
                // Fill the first row of boxes
                boxes[0][j] = j + 1;

                // Store all candidates of each box
                allCandidates.Add(j + 1);
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
            Resolve(1, 0, continueFromLastStop);
        }

        /// <summary>
        /// Try to solve the box (<paramref name="continueFromLastStop"/>, <paramref name="col"/>)
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        /// <param name="continueFromLastStop">
        /// true: continue generating from the last stop
        /// false: generate from start
        /// </param>
        private void Resolve(int row, int col, bool continueFromLastStop)
        {
            if (isStopped)
            {
                return;
            }

            var candidates = GetCandidates(row, col);
            foreach (var val in candidates)
            {
                // Put current candidate to the box
                boxes[row][col] = val;

                // If the current box is changed then the candidates of next boxes need to be recaculated
                // So we need to reset save contents of those box
                if (continueFromLastStop && boxes[row][col] != savedBoxes[row][col])
                {
                    if (!IsLastBox(row, col))
                    {
                        ResetNexBoxOfSavedContents(row, col);
                    }
                }

                if (IsLastBox(row, col))
                {
                    // Found a solution then call processor
                    if (!processor(boxes))
                    {
                        isStopped = true;

                        //Save current state to use for resuming
                        SaveCurrentBoxes();
                        break;
                    }
                }
                else
                {
                    // Resolve the next box
                    (int nextRow, int nextCol) = GetNextBox(row, col);
                    Resolve(nextRow, nextCol, continueFromLastStop);
                }
            }

            // The recursive will go back to the previous box at this point, clear the current box
            boxes[row][col] = EMPTY;
        }


        /// <summary>
        /// Reset saved boxes of the next box of box (<paramref name="row"/>, <paramref name="col"/>)
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        private void ResetNexBoxOfSavedContents(int row, int col)
        {
            (row, col) = GetNextBox(row, col);
            savedBoxes[row][col] = 1;
        }

        /// <summary>
        /// Get values that can be put to box (<paramref name="row"/>, <paramref name="col"/>)
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        /// <returns>values can be put to box (<paramref name="row"/>, <paramref name="col"/>)</returns>
        private List<int> GetCandidates(int row, int col)
        {
            return allCandidates
                .Except(GetColumn(col))
                .Except(boxes[row])
                .Except(GetBlockContains(row, col))
                .SkipWhile(r => r < savedBoxes[row][col])
                .OrderBy(r => r)
                .ToList();
        }

        /// <summary>
        /// Get all the values of a column with the index of <paramref name="col"/>
        /// </summary>
        /// <param name="col">horizontal dimension</param>
        /// <returns>all values of the column</returns>
        private IEnumerable<int> GetColumn(int col)
        {
            for (int i = 0; i < Size; i++)
            {
                yield return boxes[i][col];
            }
        }

        /// <summary>
        /// Get all the values of small block that contains box (<paramref name="row"/>, <paramref name="col"/>)
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        /// <returns>all values of the block</returns>
        private IEnumerable<int> GetBlockContains(int row, int col)
        {
            int startRow = (row / Y) * Y;
            int startCol = (col / X) * X;
            for (int i = startRow; i < startRow + Y; i++)
            {
                for (int j = startCol; j < startCol + X; j++)
                {
                    yield return boxes[i][j];
                }
            }
        }

        /// <summary>
        /// Get the next box of the box given by <paramref name="row"/> and <paramref name="col"/>
        /// from top to bottom and left to right
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        /// <returns>dimension of the next box</returns>
        private (int Row, int Col) GetNextBox(int row, int col)
        {
            if (col == Size - 1)
            {
                return (row + 1, 0);
            }

            return (row, col + 1);
        }

        /// <summary>
        /// Check if the box given by <paramref name="row"/> and <paramref name="col"/> is the last box in the game board
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        /// <returns>true: the box is the last box, false: otherwise</returns>
        private bool IsLastBox(int row, int col) => row == Size - 1 && col == Size - 1;

        /// <summary>
        /// Save the last state of game board. The saved information will be used to resume the generator
        /// </summary>
        private void SaveCurrentBoxes()
        {
            // Copy all values from boxes to savedBoxes
            for (int i = 0; i < boxes.Length; i++)
            {
                Array.Copy(boxes[i], savedBoxes[i], boxes[i].Length);
            }

            // Make the saved boxes go to next state, so that the last result won't be outputted duplicately
            for (int i = Size - 1; i >= 0; i--)
            {
                for (int j = Size - 1; j >= 0; j--)
                {
                    if (savedBoxes[i][j] < Size - 1)
                    {
                        savedBoxes[i][j]++;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Create a 2-dimension array and initilize with the value of <paramref name="initValue"/>
        /// </summary>
        /// <param name="n">dimension 1</param>
        /// <param name="m">dimension 2</param>
        /// <param name="initValue">value to fill the array</param>
        /// <returns>an array</returns>
        private static T[][] CreateArray<T>(int n, int m, T initValue)
        {
            T[][] arr = new T[n][];
            for (int i = 0; i < n; i++)
            {
                arr[i] = Enumerable.Repeat(initValue, m).ToArray();
            }

            return arr;
        }


    }
}
