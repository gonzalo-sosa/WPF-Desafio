using System.Windows.Input;

namespace Incident.WPF.Commands
{

    /// <summary>
    /// Constructor with execute and canExecute logic.
    /// </summary>
    public class RelayCommand(Action<object?> execute, Predicate<object?>? canExecute) : ICommand
    {
        private readonly Action<object?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Predicate<object?>? _canExecute = canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Constructor for commands that are always executable.
        /// </summary>
        public RelayCommand(Action<object?> execute) : this(execute, null) { }
        public bool CanExecute(object? parameter) =>
            _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) =>
            _execute(parameter);
    }
}
