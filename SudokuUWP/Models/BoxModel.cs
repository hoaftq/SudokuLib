// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Game;
using SudokuUWP.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SudokuUWP.Models
{
    public class BoxModel : BindableObject
    {
        public GameBox BoxValue { get; set; }

        public int Value => BoxValue.Value;

        public int? DisplayValue
        {
            get => BoxValue.DisplayValue;
            set
            {
                BoxValue.DisplayValue = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsFixed => BoxValue.IsFixed;

        public bool IsInvalid => BoxValue.IsInvalid;

        private double width = double.NaN;

        public double Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }

        private double height = double.NaN;

        public double Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }


        public Brush NormalBackground { get; set; } = new SolidColorBrush(Colors.White);

        public Brush InvalidBackground { get; set; } = new SolidColorBrush(Colors.LightCoral);

        public Brush Background => IsInvalid ? InvalidBackground : NormalBackground;


        public Brush NormalForeground { get; set; } = new SolidColorBrush(Colors.DarkBlue);

        public Brush FixedForeground { get; set; } = new SolidColorBrush(Colors.Black);

        public Brush Foreground => BoxValue.IsFixed ? FixedForeground : NormalForeground;


        public Thickness BorderThickness { get; set; } = new Thickness(1);

        public Brush BorderBrush { get; set; } = new SolidColorBrush(Colors.LightGreen);

        public double FontSize { get; set; } = 40;

        public bool IsSelected { get; set; } = false;


        public int X { get; set; }

        public int Y { get; set; }
    }
}
