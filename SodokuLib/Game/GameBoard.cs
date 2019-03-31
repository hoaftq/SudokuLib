// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SudokuLib.Game
{
    public class GameBoard
    {
        public int X { get; }

        public int Y { get; }

        public int Size => X * Y;

        // We don't want this list to be added or removed from outside
        // However they can still add or remove items from this list by casting this to a List
        // We can use List.AsReadonly from .NET Standard 1.3
        public IReadOnlyList<GameBox> Boxes { get; } = new List<GameBox>();

        public GameBoard(int x, int y)
        {
            X = x;
            Y = y;
            for (int i = 0; i < Size * Size; i++)
            {
                ((List<GameBox>)Boxes).Add(new GameBox());
            }
        }

        public GameBox Box(int row, int col)
        {
            ValidateDimensionParam(row, nameof(row));
            ValidateDimensionParam(col, nameof(col));
            return Boxes[row * Size + col];
        }

        public GameBox this[int row, int col] => Box(row, col);

        public IEnumerable<GameBox> Row(int row)
        {
            ValidateDimensionParam(row, nameof(row));
            return Boxes.Where((r, index) => index / Size == row);
        }

        public IEnumerable<GameBox> Column(int col)
        {
            ValidateDimensionParam(col, nameof(col));
            return Boxes.Where((r, index) => index % Size == col);
        }

        public IEnumerable<GameBox> Block(int row, int col)
        {
            ValidateDimensionParam(row, nameof(row));
            ValidateDimensionParam(col, nameof(col));

            int startRow = row - row % Y, endRow = startRow + Y;
            int startCol = col - col % X, endCol = startCol + X;

            return Boxes.Where((r, index) =>
            {
                int _row = index / Size;
                int _col = index % Size;
                return startRow <= _row && _row < endRow && startCol <= _col && _col < endCol;
            });
        }

        public bool IsEndGame()
        {
            return Boxes.All(r => r.DisplayValue != null && !r.IsInvalid);
        }

        protected bool ValidateRow(int row) =>
            ValidateBoxes(Row(row), (box) => box.IsInvalidRow = false, (box) => box.IsInvalidRow = true);

        protected bool ValidateColumn(int col) =>
            ValidateBoxes(Column(col), (box) => box.IsInvalidCol = false, (box) => box.IsInvalidCol = true);

        protected bool ValidateBlockContains(int row, int col) =>
            ValidateBoxes(Block(row, col), (box) => box.IsInvalidBlock = false, (box) => box.IsInvalidBlock = true);

        private bool ValidateBoxes(IEnumerable<GameBox> boxes, Action<GameBox> validAction, Action<GameBox> invalidAction)
        {
            bool isValid = true;
            foreach (var box in boxes)
            {
                validAction(box);
            }

            var dirtyBoxes = boxes
               .Where(r => r.DisplayValue != null)
               .OrderBy(r => r.DisplayValue)
               .ToList();
            for (int i = 0; i < dirtyBoxes.Count - 1; i++)
            {
                if (dirtyBoxes[i].DisplayValue == dirtyBoxes[i + 1].DisplayValue)
                {
                    if (!dirtyBoxes[i].IsFixed)
                    {
                        invalidAction(dirtyBoxes[i]);
                    }

                    if (!dirtyBoxes[i + 1].IsFixed)
                    {
                        invalidAction(dirtyBoxes[i + 1]);
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        [Conditional("DEBUG")]
        protected void ValidateDimensionParam(int value, string name)
        {
            if (value < 0 || value >= Size)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
