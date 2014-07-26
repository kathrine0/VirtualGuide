using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;

namespace VirtualGuide.Mobile.Common
{
    class MessageBoxHelper
    {
        public static void Show(string content, string title)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);
            messageDialog.ShowAsync();
        }
    }
}
