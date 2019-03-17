// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib.Generator
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
    public class GeneratorBase
    {
        public const int EMPTY = 0;

        private List<int> allCandidates = new List<int>();

        private readonly GameOutputDelegate processor;

        private readonly int[][] savedBoxes;

        protected int[][] boxes;

        // true means there is a request to stop the generator
        protected bool isStopped;

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
        public GeneratorBase(int x, int y, GameOutputDelegate processor, int[][] savedBoxes = null)
        {
            X = x;
            Y = y;
            Size = X * Y;
            this.processor = processor ?? (r => false);
            this.savedBoxes = savedBoxes ?? CreateArray(Size, Size, 1);

            // Initialize boxes
            boxes = CreateArray(Size, Size, EMPTY);

            for (int i = 0; i < Size; i++)
            {
                // Store all candidates of each box
                allCandidates.Add(i + 1);
            }
        }

        /// <summary>
        /// Solve the game from the box (<paramref name="continueFromLastStop"/>, <paramref name="col"/>).
        /// isStopped must be set to false before calling this function
        /// </summary>
        /// <param name="row">vertical dimension to start resolving</param>
        /// <param name="col">horizontal dimension to start resolving</param>
        /// <param name="continueFromLastStop">
        /// true: continue generating from the last stop
        /// false: generate from start
        /// </param>
        public void Solve(int row, int col, bool continueFromLastStop)
        {
            if (isStopped)
            {
                return;
            }

            // The box is a preset box
            if (boxes[row][col] != EMPTY)
            {
                // Ignore the box, solve the next box
                (int nextRow, int nextCol) = GetNextBox(row, col);
                Solve(nextRow, nextCol, continueFromLastStop);
                return;
            }

            var candidates = GetCandidates(row, col);
            foreach (var val in candidates)
            {
                // Put current candidate to the box
                boxes[row][col] = val;

                // If the current box is changed then the candidates of next boxes need to be recaculated
                // So we need to reset save contents of those boxes
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
                    // Solve the next box
                    (int nextRow, int nextCol) = GetNextBox(row, col);
                    Solve(nextRow, nextCol, continueFromLastStop);
                }
            }

            // The recursive will go back to the previous box at this point, clear the current box
            boxes[row][col] = EMPTY;
        }

        /// <summary>
        /// Get the next box of the box given by <paramref name="row"/> and <paramref name="col"/>
        /// from top to bottom and left to right
        /// </summary>
        /// <param name="row">vertical dimension</param>
        /// <param name="col">horizontal dimension</param>
        /// <returns>dimension of the next box</returns>
        protected (int Row, int Col) GetNextBox(int row, int col)
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
        protected bool IsLastBox(int row, int col) => row == Size - 1 && col == Size - 1;

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
        /// Create a 2-dimension array and initilize with the value of <paramref name="initValue"/>
        /// </summary>
        /// <param name="n">dimension 1</param>
        /// <param name="m">dimension 2</param>
        /// <param name="initValue">value to fill the array</param>
        /// <returns>an array</returns>
        protected static T[][] CreateArray<T>(int n, int m, T initValue)
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
