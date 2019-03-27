// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuUWP.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SudokuUWP.Controls
{
    public sealed partial class Box : UserControl
    {
        public BoxModel Value
        {
            get { return (BoxModel)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);

                IsEnabled = !value.IsFixed;
            }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(BoxModel), typeof(Box), new PropertyMetadata(null));

        public Box()
        {
            this.InitializeComponent();
        }
    }
}
