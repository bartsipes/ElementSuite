namespace ElementSuite.Addin.Interface
{
    using System.Collections.Generic;

    /// <summary>
    /// A menu item that may be added to the menu of the main Element Suite user interface.
    /// </summary>
    public interface IMenuExtension
    {
        /// <summary>
        /// Name that will be displayed on the menu
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Command to be executed when the menu item is selected. Setting the <see cref="Command"/> property overrides the <see cref="SubMenus"/> property.
        /// </summary>
        System.Windows.Input.ICommand Command { get; }

        /// <summary>
        /// List of menu items to be displayed as a sub-menus under this menu item. This collection is ignored if the <see cref="Command"/> property is set.
        /// </summary>
        IEnumerable<IMenuExtension> SubMenus { get; }
    }
}