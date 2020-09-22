using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace HandHistories.Parser.WPFTestApp.ViewModels.Base
{
    public class RelayCommand : ICommand
    {
        Action<object> Command;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> command) => Command = command;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Command?.Invoke(parameter);
    }
}
