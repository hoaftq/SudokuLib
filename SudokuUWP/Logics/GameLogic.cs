// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Game;
using SudokuUWP.Models;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SudokuUWP.Logics
{
    public class GameLogic
    {
        private const double DEFAULT_THICKNESS =1;

        private const double BLOCK_THICKNESS = 2;

        private readonly Brush DarkBlockBrush = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));

        private readonly Brush LightBlockBrush = new SolidColorBrush(Colors.White);


        private SudokuGame game;


        public int X => game.X;

        public int Y => game.Y;

        public GameLevel Level => game.Level;

        public int Size => game.Size;

        public List<BoxModel> Boxes { get; private set; }


        public GameLogic(int x, int y, GameLevel level)
        {
            game = new SudokuGame(x, y, level);
            CreateGameBoard();
        }

        public void NewGame(GameLevel? level = null)
        {
            if (level == null)
            {
                game.NewGame();
            }
            else
            {
                game.NewGame(level.Value);
            }

            ApplyNewGame();
        }

        public bool ValidateWhenChangeAt(BoxModel box)
        {
            int index = Boxes.IndexOf(box);
            return game.ValidateWhenChangeAt(index / Size, index % Size);
        }

        public bool IsEndGame() => game.IsEndGame();

        private void CreateGameBoard()
        {
            Boxes = new List<BoxModel>();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var box = new BoxModel()
                    {
                        BorderThickness = CreateThickness(i, j),
                        NormalBackground = CreateBackground(i, j),
                        X = X,
                        Y = Y
                    };

                    Boxes.Add(box);
                }
            }
        }

        private void ApplyNewGame()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Boxes[i * Size + j].BoxValue = game[i, j];
                }
            }
        }

        private Thickness CreateThickness(int row, int col)
        {
            double left = DEFAULT_THICKNESS;
            double top = DEFAULT_THICKNESS;
            double right = DEFAULT_THICKNESS;
            double bottom = DEFAULT_THICKNESS;

            if (row % Y == 0)
            {
                top = BLOCK_THICKNESS;
            }
            else if (row % Y == Y - 1)
            {
                bottom = BLOCK_THICKNESS;
            }

            if (col % X == 0)
            {
                left = BLOCK_THICKNESS;
            }
            else if (col % X == X - 1)
            {
                right = BLOCK_THICKNESS;
            }

            return new Thickness(left, top, right, bottom);
        }

        private Brush CreateBackground(int row, int col)
        {
            if ((row / Y + col / X) % 2 == 0)
            {
                return DarkBlockBrush;
            }

            return LightBlockBrush;
        }
    }
}
