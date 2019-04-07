// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using System;
using Windows.UI.Xaml.Data;

namespace SudokuUWP.Converters
{
    class PlayingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var playingTime = (TimeSpan)value;
            if (playingTime.Hours == 0)
            {
                return $"{playingTime.Minutes:00}:{playingTime.Seconds:00}";
            }

            return $"{playingTime.Hours}:{playingTime.Minutes:00}:{playingTime.Seconds:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
