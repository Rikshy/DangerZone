﻿using System;
using System.Windows.Input;

namespace DangerZone.Helper
{
    public class LightCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action execute;

        public LightCommand(Action execute, Predicate<object> canExecute = null)
        {
            this.canExecute = canExecute ?? delegate { return true; };
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) { return canExecute(parameter); }

        public void Execute(object parameter) { execute(); }
    }
}
