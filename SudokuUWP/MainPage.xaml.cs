// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuUWP.Models;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SudokuUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel = new MainPageViewModel();

        public MainPage()
        {
            this.InitializeComponent();

            foreach (var level in DispGameLevel.Values)
            {
                levelMenu.Items.Add(new MenuFlyoutItem()
                {
                    Text = level.Text,
                    Command = ViewModel.LevelChangedCommand,
                    CommandParameter = level
                });
            }
        }

        private void ItemsWrapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //var itemsPanel = sender as ItemsWrapGrid;
            //itemsPanel.MaximumRowsOrColumns = ViewModel.Size;
        }

        private void Box_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void NumbersBoard_Selected(int? selectedValue, object parameter)
        {
            (parameter as Flyout).Hide();

            var selectedBox = gameBoard.SelectedItem as BoxModel;
            ViewModel.ValidateWhenChangeAt(selectedBox);
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var selectedValue = gameBoard.SelectedItem as BoxModel;
            if (selectedValue == null)
            {
                return;
            }

            if (selectedValue.IsFixed)
            {
                return;
            }

            // Backspace or delete key is pressed then clear the current box
            if (e.Key == VirtualKey.Back || e.Key == VirtualKey.Delete)
            {
                selectedValue.DisplayValue = null;
            }

            // Pressed key is not a valid digit then ignore it
            if (e.Key <= VirtualKey.Number0 || e.Key > VirtualKey.Number0 + ViewModel.Size)
            {
                return;
            }

            // Store the valid digit
            selectedValue.DisplayValue = e.Key - VirtualKey.Number0;

            ViewModel.ValidateWhenChangeAt(selectedValue);
        }
    }
}
