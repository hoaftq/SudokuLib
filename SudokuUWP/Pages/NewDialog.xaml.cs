// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SudokuUWP.Pages
{

    public sealed partial class NewGameDialog : ContentDialog
    {
        private List<int> Sizes => new List<int>() { 2, 3, 4 };

        private List<GameLevel> Levels = Enum.GetValues(typeof(GameLevel)).Cast<GameLevel>().ToList();

        public int X { get; set; } = 3;

        public int Y { get; set; } = 3;

        public GameLevel Level { get; set; } = GameLevel.Medium;

        public NewGameDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
