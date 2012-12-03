using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ElementSuite.Common
{
    /// <summary>
    /// Tab for working with the Element Suite UI workbench. This tab extends the regular TabItem class to provide
    /// a close button.
    /// </summary>
    public class WorkbenchTab : TabItem
    {
        /// <summary>
        /// Creates a new tab item with a close button.
        /// </summary>
        /// <param name="title">Title to display in the tab.</param>
        public WorkbenchTab(string title) : base()
        {
            var headerStackPanel = new StackPanel();
            headerStackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            headerStackPanel.Children.Add(new TextBlock()
            {
                Margin = new Thickness(0d, 0d, 5d, 0d),
                Text = title,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            });
            var close = new Button();
            close.Content = " X ";
            close.Click += (s, e) => RaiseCloseEvent();
            headerStackPanel.Children.Add(close);
            Header = headerStackPanel;
        }

        /// <summary>
        /// Create a custom routed event by first registering a RoutedEventID 
        /// This event uses the bubbling routing strategy 
        /// </summary>
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
            "Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WorkbenchTab));

        /// <summary>
        /// Provide CLR accessors for the event 
        /// </summary>
        public event RoutedEventHandler Close;

        /// <summary>
        /// This method raises the close event 
        /// </summary>
        protected void RaiseCloseEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(CloseEvent, this);
            RaiseEvent(newEventArgs);
        }
    }
}
