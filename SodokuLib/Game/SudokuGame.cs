// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Generator;
using System;

namespace SudokuLib.Game
{
    public class SudokuGame : GameBoard
    {
        private const double EASY_OPEN_BOXES_RATE = 2.6;

        private const double MEDIUM_OPEN_BOXES_RATE = 3.2;

        private const double HARD_OPEND_BOXES_RATE = 3.8;

        private const double VERY_HARD_OPEN_BOXES_RATE = 4.4;

        public GameLevel Level { get; private set; }

        private SudokuGenerator generator;

        public SudokuGame(int x, int y, GameLevel level = GameLevel.Medium) : base(x, y)
        {
            Level = level;
            generator = new SudokuGenerator(x, y, ResultHandler);
        }

        public void NewGame() => generator.Generate(true);

        public void NewGame(GameLevel level)
        {
            Level = level;
            NewGame();
        }

        public bool ValidateWhenChangeAt(int row, int col)
        {
            ValidateDimensionParam(row, nameof(row));
            ValidateDimensionParam(col, nameof(col));

            return ValidateRow(row) & ValidateColumn(col) & ValidateBlockContains(row, col);
        }

        private bool ResultHandler(int[][] result)
        {
            var mask = generator.CreateRandomMask(GetNumberOfOpenBoxes());
            var permutation = generator.GetPermutation();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int value = permutation[result[i][j] - 1];
                    var box = Box(i, j);
                    box.Value = value;
                    box.DisplayValue = mask[i][j] ? (int?)value : null;
                    box.IsFixed = mask[i][j];
                    box.IsInvalidBlock = box.IsInvalidCol = box.IsInvalidRow = false;
                }
            }

            return false;
        }

        private int GetNumberOfOpenBoxes()
        {
            switch (Level)
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

    }
}
