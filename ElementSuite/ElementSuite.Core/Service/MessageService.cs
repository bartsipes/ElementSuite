using ElementSuite.Common;
using ElementSuite.Common.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElementSuite.Core.Service
{
    public sealed class MessageService : IMessageService
    {
        public MessageService()
        {
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Element Suite", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowException(string message, Exception exception)
        {
            MessageBox.Show(message, "Element Suite", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(message, "Element Suite", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public bool AskQuestion(string message, string caption)
        {
            return MessageBox.Show(message, "Element Suite", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
