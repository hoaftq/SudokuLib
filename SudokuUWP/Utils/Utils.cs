// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using System;
using Windows.UI.Xaml;

namespace SudokuUWP.Utils
{
    public class Utils
    {
        public static double GetWindowSize() =>
            Math.Min(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
    }
}
