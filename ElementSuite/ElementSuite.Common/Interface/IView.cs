namespace ElementSuite.Common.Interface
{
    /// <summary>
    /// Interface for decoupling the key methods from <see cref="System.Windows.Window"/> classes.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Opens the view and returns when the opened view is closed. See http://msdn.microsoft.com/en-us/library/system.windows.window.showdialog.aspx
        /// for more details.
        /// </summary>
        /// <returns>Nullable boolean indicating of the view was accepted or canceled.</returns>
        bool? ShowDialog();
    }
}
