// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SudokuUWP.Controls
{
    public sealed class NumbersBoard : Grid
    {
        private readonly Brush NormalBorderBrush = new SolidColorBrush(Colors.DarkGray);

        private readonly Brush SelectedBorderBrush = new SolidColorBrush(Colors.DarkBlue);

        private readonly Brush ForegroundBrush = new SolidColorBrush(Colors.Black);

        public int X
        {
            get { return (int)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(int), typeof(NumbersBoard), new PropertyMetadata(3, new PropertyChangedCallback(XChangedHandler)));

        private static void XChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO
            (d as NumbersBoard).Initialize();
        }

        public int Y
        {
            get { return (int)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(int), typeof(NumbersBoard), new PropertyMetadata(3));


        public int? SelectedNumber
        {
            get { return (int?)GetValue(SelectedNumberProperty); }
            set { SetValue(SelectedNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedNumberProperty =
            DependencyProperty.Register("SelectedNumber", typeof(object), typeof(NumbersBoard), new PropertyMetadata(null));


        public double BoxWidth
        {
            get { return (double)GetValue(BoxWidthProperty); }
            set { SetValue(BoxWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxWidthProperty =
            DependencyProperty.Register("BoxWidth", typeof(double), typeof(NumbersBoard), new PropertyMetadata(0d));


        public double BoxHeight
        {
            get { return (double)GetValue(BoxHeightProperty); }
            set { SetValue(BoxHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxHeightProperty =
            DependencyProperty.Register("BoxHeight", typeof(double), typeof(NumbersBoard), new PropertyMetadata(0d));


        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(NumbersBoard), new PropertyMetadata(14));

        public event Action<int?> Selected;

        public NumbersBoard()
        {
            Loaded += NumbersBoard_Loaded;
        }

        private void NumbersBoard_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();

            for (int i = 0; i <= Y; i++)
            {
                RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }

            for (int i = 0; i < X; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
            }

            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    int value = (j + 1) + i * X;
                    var button = CreateButton(value, false);
                    button.Tapped += Button_Tapped;

                    Children.Add(button);
                    SetRow(button, i);
                    SetColumn(button, j);
                }
            }

            var clearButton = CreateButton("Clear", true);
            clearButton.Tapped += ClearButton_Tapped;
            Children.Add(clearButton);
            SetRow(clearButton, Y);
            SetColumn(clearButton, 0);
            SetColumnSpan(clearButton, X);
        }

        private void ClearButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelectedNumber = null;
            Selected?.Invoke(SelectedNumber);
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as Button;
            SelectedNumber = Convert.ToInt32(button.Content);
            Selected?.Invoke(SelectedNumber);
        }

        private Button CreateButton(object content, bool isClearButton)
        {
            var button = new Button()
            {
                Content = content,
                Margin = new Thickness(2),
                Padding = new Thickness(0),
                FontSize = this.FontSize,
                Height = BoxHeight,
                Background = new SolidColorBrush(),
                BorderThickness = new Thickness(2),
                Foreground = ForegroundBrush
            };

            if (isClearButton)
            {
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.BorderBrush = SelectedNumber == null ? SelectedBorderBrush : NormalBorderBrush;
            }
            else
            {
                button.Width = BoxWidth;
                button.BorderBrush = (int)content == SelectedNumber ? SelectedBorderBrush : NormalBorderBrush;
            }

            return button;
        }
    }
}
