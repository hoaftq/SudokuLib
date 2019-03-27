// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using System;
using System.Windows.Input;

namespace SudokuUWP.Utils
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> action;

        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> action) => this.action = action;

        public DelegateCommand(Action<object> action, Func<object, bool> canExecute) : this(action)
            => this.canExecute = canExecute;

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => action(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged(this, EventArgs.Empty);
    }
}
