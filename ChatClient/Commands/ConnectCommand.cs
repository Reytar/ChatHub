using System;
using System.Windows.Input;

namespace ChatClient.Commands
{
    public class ConnectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public ConnectCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
