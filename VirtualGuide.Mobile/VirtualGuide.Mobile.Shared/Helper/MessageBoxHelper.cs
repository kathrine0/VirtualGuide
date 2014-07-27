using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;

namespace VirtualGuide.Mobile.Helper
{
    class MessageBoxHelper
    {
        public static void Show(string content, string title)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);
            messageDialog.ShowAsync();
        }

        public static void Show(string content)
        {
            MessageDialog messageDialog = new MessageDialog(content);
            messageDialog.ShowAsync();
        }
    }
}
