using Windows.Storage;
using Windows.UI.Popups;

namespace VirtualGuide.Mobile.Helper
{
    class MessageBoxHelper
    {
        private static ApplicationDataContainer _appSettings = ApplicationData.Current.LocalSettings;
       
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

        public static void ShowNoLocation()
        {
            if (!LocalDataHelper.KeyExists("locationMsg") || LocalDataHelper.GetKeyValue<bool>("locationMsg") == false)
            {
                LocalDataHelper.SetValue("locationMsg", true); 
                Show("Location feature is turned off");
            }
        }
    }
}
