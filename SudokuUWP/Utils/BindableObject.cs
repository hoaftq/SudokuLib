// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SudokuUWP.Utils
{
    public class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void SetProperty<T>(ref T property, T value, [CallerMemberName]string propertyName = "")
        {
            property = value;
            NotifyPropertyChanged(propertyName);
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
