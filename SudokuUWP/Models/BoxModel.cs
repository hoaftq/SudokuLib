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
        private GameBox boxValue;

        public GameBox BoxValue
        {
            get => boxValue;
            set
            {
                boxValue = value;
                NotifyBoxStateChanged();
            }
        }

        public int Value => boxValue.Value;

        public int? DisplayValue
        {
            get => boxValue?.DisplayValue;
            set
            {
                boxValue.DisplayValue = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsFixed => boxValue?.IsFixed ?? false;

        public bool IsInvalid => boxValue?.IsInvalid ?? false;

        public bool IsEnabled => !boxValue.IsFixed;


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

        public Brush Foreground => (BoxValue?.IsFixed ?? false) ? FixedForeground : NormalForeground;


        public Thickness BorderThickness { get; set; } = new Thickness(1);

        public Brush BorderBrush { get; set; } = new SolidColorBrush(Colors.LightBlue);

        public double FontSize { get; set; } = 40;

        public bool IsSelected { get; set; } = false;


        public int X { get; set; }

        public int Y { get; set; }


        public void NotifyBoxStateChanged()
        {
            NotifyPropertyChanged(nameof(Value));
            NotifyPropertyChanged(nameof(DisplayValue));
            NotifyPropertyChanged(nameof(IsFixed));
            NotifyPropertyChanged(nameof(IsInvalid));
            NotifyPropertyChanged(nameof(Background));
            NotifyPropertyChanged(nameof(Foreground));
            NotifyPropertyChanged(nameof(IsEnabled));
        }

        public void ClearInvalidState()
        {
            boxValue.IsInvalidRow = boxValue.IsInvalidCol = boxValue.IsInvalidBlock = false;
            NotifyPropertyChanged(nameof(IsInvalid));
            NotifyPropertyChanged(nameof(Background));
            NotifyPropertyChanged(nameof(IsEnabled));
        }
    }
}
