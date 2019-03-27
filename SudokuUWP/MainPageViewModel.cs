// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Game;
using SudokuUWP.Logics;
using SudokuUWP.Models;
using SudokuUWP.Pages;
using SudokuUWP.Utils;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SudokuUWP
{
    public class MainPageViewModel : BindableObject
    {
        private GameLogic gameLogic;

        public int X => gameLogic.X;

        public int Y => gameLogic.Y;

        public int Size => gameLogic.Size;

        public GameLevel Level => gameLogic.Level;

        public List<BoxModel> Boxes => gameLogic.Boxes;


        public void ValidateWhenChangeAt(BoxModel box)
        {
            gameLogic.ValidateWhenChangeAt(box);
            Boxes.ForEach(b => b.NotifyAllPropertiesChanged());
        }

        public DelegateCommand NewGameCommand => new DelegateCommand(parameter =>
        {
            gameLogic.NewGame();
            Boxes.ForEach(b => b.NotifyAllPropertiesChanged());
        });

        public DelegateCommand NewGameWithOptionsCommand => new DelegateCommand(async parameter =>
        {
            var newGameDialog = new NewGameDialog()
            {
                X = X,
                Y = Y,
                Level = Level
            };
            if (await newGameDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                gameLogic = new GameLogic(newGameDialog.X, newGameDialog.Y, newGameDialog.Level);
                NotifyAllPropertiesChanged();
            }
        });

        public MainPageViewModel()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            int.TryParse(localSettings.Values["BlockWidth"] as string, out int x);
            if (x < 2 || x > 4)
            {
                x = 3;
            }

            int.TryParse(localSettings.Values["BlockHeight"] as string, out int y);
            if (y < 2 || y > 4)
            {
                y = 3;
            }

            GameLevel level = GameLevel.Medium;

            gameLogic = new GameLogic(x, y, level);
        }
    }
}
