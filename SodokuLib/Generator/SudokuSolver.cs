// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib.Generator
{
    public class SudokuSolver : GeneratorBase
    {
        public SudokuSolver(int x, int y, GameOutputDelegate processor)
            : base(x, y, processor, null)
        {
        }

        public bool[][] InitializeBoard(int[][] presetBoard)
        {
            // Validate before setting
            var errors = CreateArray(Size, Size, false);

            for (int i = 0; i < Size; i++)
            {
                Array.Copy(presetBoard[i], boxes[i], Size);
            }

            for (int i = 0; i < Size; i++)
            {
                var row = GetBoxes(i, i, 0, Size - 1);
                ValidateSingleValue(row);
                Validate(row);
            }

            for (int j = 0; j < Size; j++)
            {
                var col = GetBoxes(0, Size - 1, j, j);
                Validate(col);
            }

            for (int i = 0; i < Size; i += Y)
            {
                for (int j = 0; j < Size; j += X)
                {
                    var block = GetBoxes(i, i + Y - 1, j, j + X - 1);
                    Validate(block);
                }
            }

            return errors;


            List<(int x, int y, int value)> GetBoxes(int rowFrom, int rowTo, int colFrom, int colTo)
            {
                var boxes = new List<(int, int, int)>();
                for (int i = rowFrom; i <= rowTo; i++)
                {
                    for (int j = colFrom; j <= colTo; j++)
                    {
                        boxes.Add((i, j, presetBoard[i][j]));
                    }
                }

                return boxes;
            }

            void Validate(List<(int x, int y, int value)> boxes)
            {
                var t = boxes.OrderBy(r => r.value).ToList();
                for (int i = 0; i < t.Count - 1; i++)
                {
                    if (t[i].value == t[i + 1].value)
                    {
                        errors[t[i].x][t[i].y] = true;
                        errors[t[i + 1].x][t[i + 1].y] = true;
                    }
                }
            }

            void ValidateSingleValue(List<(int x, int y, int value)> boxes)
            {
                foreach (var box in boxes.Where(r => r.value < 1 || r.value > Size))
                {
                    errors[box.x][box.y] = true;
                }
            }
        }

        public void Solve(bool continueFromLastStop = false)
        {
            isStopped = false;
            Solve(0, 0, continueFromLastStop);
        }
    }
}
