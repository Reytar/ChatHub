﻿using System;
using System.Windows.Input;

namespace ChatClient.Commands
{
    public class SendCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public SendCommand(Action execute)
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
