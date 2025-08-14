using System;
using System.Windows.Input;

namespace CalculatorPractice
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) { return true; }

        public RelayCommand(Action<T> execute) => _execute = execute;


        public void Execute(object parameter) => _execute((T)parameter);

    }
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(_ => execute()) { }

        public RelayCommand(Action<object> execute) : base(execute)
        {
        }
    }
}