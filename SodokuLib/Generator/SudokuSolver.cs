// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;

namespace SudokuLib.Generator
{
    public class SudokuSolver : GeneratorBase
    {
        public SudokuSolver(int x, int y, GameOutputDelegate processor, int[][] presetBoard)
            : base(x, y, processor, null)
        {
            for (int i = 0; i < Size; i++)
            {
                Array.Copy(presetBoard[i], boxes[i], Size);
            }
        }

        public bool Validate()
        {
            // TODO not implement yet
            return false;
        }

        public void Solve(bool continueFromLastStop = false)
        {
            isStopped = false;
            Solve(0, 0, continueFromLastStop);
        }
    }
}
