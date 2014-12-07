using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace VirtualGuide.Mobile.Helper
{
    class MessageBoxHelper
    {
        private static LocalDataHelper localDataHelper = new LocalDataHelper();
       
        public static async void Show(string content, string title)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await messageDialog.ShowAsync();
            });


        }

        public static async void Show(string content)
        {
            Show(content, "");
        }

        public static async void ShowWithCommands(string content, string title, IList<UICommand> commands, uint defaultCommandIndex = 0, uint cancelCommandIndex = 1)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);
            
            foreach(var command in commands)
            {
                messageDialog.Commands.Add(command);
            }

            messageDialog.DefaultCommandIndex = defaultCommandIndex;
            messageDialog.CancelCommandIndex = cancelCommandIndex;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await messageDialog.ShowAsync();
            });
        }

        public static void ShowNoLocation()
        {
            if (!localDataHelper.KeyExists(LocalDataHelper.LOCATION_MSG) || localDataHelper.GetValue<bool>(LocalDataHelper.LOCATION_MSG) == false)
            {
                localDataHelper.SetValue(LocalDataHelper.LOCATION_MSG, true); 
                Show(App.ResLoader.GetString("TurnOnLocationInstruction"), App.ResLoader.GetString("NoLocation"));
            }
        }
    }
}
