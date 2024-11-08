using System.Windows.Input;

namespace Family_Office.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        // Constructor for commands that always execute
        public RelayCommand(Action execute) : this(execute, null) { }

        // Constructor for commands with a CanExecute function
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // ICommand method: Determines whether the command can execute
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        // ICommand method: Executes the command
        public void Execute(object parameter)
        {
            _execute();
        }

        // Event to notify when CanExecute changes
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

}
