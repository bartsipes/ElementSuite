using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ServiceModel;
using ElementSuite.Moon.Core;
using System.Windows;

/* ***** BEGIN LICENSE BLOCK *****
 * Version: MIT License
 *
 * Copyright (c) 2010 Michael Sorens http://www.simple-talk.com/author/michael-sorens/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * ***** END LICENSE BLOCK *****
 */

namespace ElementSuite.Moon.UI
{
    /// <summary>
    /// Framework for running application as a tray app.
    /// </summary>
    /// <remarks>
    /// Tray app code adapted from "Creating Applications with NotifyIcon in Windows Forms", Jessica Fosler,
    /// http://windowsclient.net/articles/notifyiconapplications.aspx
    /// </remarks>
    public class MoonApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "ElementSuite.Moon.UI.logo.ico";
        private static readonly string DefaultTooltip = "Element Suite Distributed Client";
        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray
        private Window help;
        private ServiceHost host;
        private bool enabled = false;

        /// <summary>
        /// This class should be created and passed into Application.Run( ... )
        /// </summary>
        public MoonApplicationContext()
        {
            components = new System.ComponentModel.Container();
            var assembly = Assembly.GetExecutingAssembly();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(assembly.GetManifestResourceStream("ElementSuite.Moon.UI.logo.ico")),
                Text = DefaultTooltip,
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.MouseClick += NotifyIcon_MouseUp;
            EnableService();
        }

        private bool EnableService()
        {
            try
            {
                host = new ServiceHost(typeof(LunarRover));
                host.Open();
                enabled = true;
                return true;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Oops! Something went wrong while trying to enable Moon. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }

        private bool DisableService()
        {
            try
            {
                if (host != null)
                    host.Close();
                enabled = false;
                return true;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Oops! Something went wrong while trying to disable Moon. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            notifyIcon.ContextMenuStrip.Items.Clear();

            if (enabled)
            {
                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&Disable", (s, args) =>
                {
                    var tsi = s as ToolStripItem;
                    tsi.Enabled = false;
                    if (DisableService())
                    {
                        notifyIcon.ContextMenuStrip.Close();
                    }
                    else
                    {
                        tsi.Enabled = true;
                    }
                }));
            }
            else
            {
                notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("E&nable", (s, args) =>
                {
                    var tsi = s as ToolStripItem;
                    tsi.Enabled = false;
                    if (EnableService())
                    {
                        notifyIcon.ContextMenuStrip.Close();
                    }
                    else
                    {
                        tsi.Enabled = true;
                    }
                }));
            }

            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&Help/About", (s, args) => ShowHelpForm()));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&Exit", ExitItem_Click));
        }

        # region Child Forms

        private void ShowHelpForm()
        {
            if (help == null)
            {
                help = new Help();
                help.Closed += (sender, e) => help = null;
                help.Show();
            }
            else { help.Activate(); }
        }

        // From http://stackoverflow.com/questions/2208690/invoke-notifyicons-context-menu
        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        # endregion Child Forms

        # region generic code framework

        /// <summary>
        /// When the application context is disposed, dispose things like the notify icon.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (help != null)
                    help.Close();

                if (notifyIcon != null)
                {
                    if (notifyIcon.ContextMenuStrip != null)
                        notifyIcon.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;
                    notifyIcon.MouseUp -= NotifyIcon_MouseUp;
                    notifyIcon.Dispose();
                }
            }
        }

        /// <summary>
        /// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitItem_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            // before we exit, let forms clean themselves up.
            if (help != null) { help.Close(); }
            if (host != null) { host.Close(); }

            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

        public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null) { item.Click += eventHandler; }

            return item;
        }
        # endregion generic code framework
    }
}