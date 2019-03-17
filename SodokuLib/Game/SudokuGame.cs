// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Generator;

namespace SudokuLib.Game
{
    public class SudokuGame : SudokuGameBase
    {
        private SudokuGenerator generator;

        public SudokuGame(int x, int y, GameLevel level = GameLevel.Medium)
            : base(x, y, level)
        {
            generator = new SudokuGenerator(x, y, ResultHandler);
        }

        public override void NewGame() => generator.Generate(true);

        private bool ResultHandler(int[][] result)
        {
            var mask = generator.CreateRandomMask(GetNumberOfOpenBoxes(Level));
            var permutation = generator.GetPermutation();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int value = permutation[result[i][j] - 1];
                    var box = Box(i, j);
                    box.Value = value;
                    box.IsFixed = mask[i][j];
                    box.DisplayValue = mask[i][j] ? (int?)value : null;
                }
            }

            return false;
        }
    }
}
