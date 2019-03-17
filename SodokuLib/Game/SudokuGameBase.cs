// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SudokuLib.Game
{
    public abstract class SudokuGameBase
    {
        private const double EASY_OPEN_BOXES_RATE = 2.6;

        private const double MEDIUM_OPEN_BOXES_RATE = 3.2;

        private const double HARD_OPEND_BOXES_RATE = 3.8;

        private const double VERY_HARD_OPEN_BOXES_RATE = 4.4;

        //private SudokuGenerator generator;

        public int X { get; }

        public int Y { get; }

        public int Size => X * Y;

        public List<GameBox> Boxes { get; } = new List<GameBox>();

        public GameLevel Level { get; private set; }


        public SudokuGameBase(int x, int y, GameLevel level = GameLevel.Medium)
        {
            //generator = new SudokuGenerator(x, y, ResultHandler);
            X = x;
            Y = y;
            Level = level;
            for (int i = 0; i < Size * Size; i++)
            {
                Boxes.Add(new GameBox());
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
            return Boxes.Where((r, index) => index / Size == 0);
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
                return startRow <= _row && _row < endRow && startCol <= _col && _col <= endCol;
            });
        }

        public abstract void NewGame();
        //=> generator.Generate(true);

        public void NewGame(GameLevel level)
        {
            Level = level;
            //generator.Generate(true);
            NewGame();
        }

        public void ValidateWhenChangeAt(int row, int col)
        {
            ValidateDimensionParam(row, nameof(row));
            ValidateDimensionParam(col, nameof(col));
            ValidateBoxes(Row(row), (box) => box.IsInvalidRow = true);
            ValidateBoxes(Column(row), (box) => box.IsInvalidCol = true);
            ValidateBoxes(Block(row, col), (box) => box.IsInvalidBlock = true);
        }

        private void ValidateBoxes(IEnumerable<GameBox> boxes, Action<GameBox> invalidAction)
        {
            var changedBoxes = boxes
               .Where(r => r.DisplayValue != null)
               .OrderBy(r => r.DisplayValue)
               .ToList();
            for (int i = 0; i < changedBoxes.Count - 1; i++)
            {
                if (changedBoxes[i].DisplayValue == changedBoxes[i + 1].DisplayValue)
                {
                    invalidAction(changedBoxes[i]);
                    invalidAction(changedBoxes[i + 1]);
                }
            }
        }

        //private bool ResultHandler(int[][] result)
        //{
        //    var mask = generator.CreateRandomMask(GetNumberOfOpenBoxes(Level));
        //    var permutation = generator.GetPermutation();

        //    for (int i = 0; i < Size; i++)
        //    {
        //        for (int j = 0; j < Size; j++)
        //        {
        //            int value = permutation[result[i][j] - 1];
        //            Boxes.Add(new GameBox()
        //            {
        //                Value = value,
        //                IsFixed = mask[i][j],
        //                DisplayValue = mask[i][j] ? (int?)value : null
        //            });
        //        }
        //    }

        //    return false;
        //}

        protected int GetNumberOfOpenBoxes(GameLevel level)
        {
            switch (level)
            {
                case GameLevel.Easy:
                    return (int)Math.Round(Size * Size / EASY_OPEN_BOXES_RATE);

                case GameLevel.Medium:
                    return (int)Math.Round(Size * Size / MEDIUM_OPEN_BOXES_RATE);

                case GameLevel.Hard:
                    return (int)Math.Round(Size * Size / HARD_OPEND_BOXES_RATE);

                case GameLevel.VeryHard:
                    return (int)Math.Round(Size * Size / VERY_HARD_OPEN_BOXES_RATE);
            }

            throw new ArgumentException();
        }

        [Conditional("DEBUG")]
        private void ValidateDimensionParam(int value, string name)
        {
            if (value < 0 || value >= Size)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
