namespace ElementSuite.Addin
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Extends the <see cref="System.Windows.Input.ICommand"/> to provide an easy means of exposing
    /// arbitrary methods to handle the execution of a command in a MVVM environment.
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a RelayCommand object specifiying the action that will be executed.
        /// </summary>
        /// <param name="execute">Action to be excuted when <see cref="System.Windows.Input.ICommand.Execute"/> is called.</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a RelayCommand object specifiying the action that will be executed as well as the
        /// predicate to determine if the excute action is available to be called.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        /// <summary>
        /// Handler for determining the current availibility of the <see cref="System.Windows.Input.ICommand.Execute"/> method.
        /// </summary>
        /// <param name="parameter">Contextual parameter provided by the invoker of the <see cref="System.Windows.Input.ICommand"/></param>
        /// <returns>Boolean indicating avaliablity of <see cref="System.Windows.Input.ICommand.Execute"/> method.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Notifies any subscribers of a change to the ability of the command to be executed.s
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Action to be excuted when <see cref="System.Windows.Input.ICommand.Execute"/> is called.
        /// </summary>
        /// <param name="parameter">Contextual parameter provided by the invoker of the <see cref="System.Windows.Input.ICommand"/></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }
}