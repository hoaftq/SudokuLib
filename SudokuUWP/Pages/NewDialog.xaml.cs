// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Game;
using SudokuUWP.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SudokuUWP.Pages
{

    public sealed partial class NewGameDialog : ContentDialog
    {
        private List<int> Sizes => new List<int>() { 2, 3, 4 };

        private List<DispGameLevel> Levels => DispGameLevel.Values;

        public int X { get; set; } = 3;

        public int Y { get; set; } = 3;

        public DispGameLevel Level { get; set; } = GameLevel.Medium;

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
