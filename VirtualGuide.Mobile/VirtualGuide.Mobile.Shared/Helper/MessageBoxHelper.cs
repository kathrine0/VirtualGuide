using Windows.Storage;
using Windows.UI.Popups;

namespace VirtualGuide.Mobile.Helper
{
    class MessageBoxHelper
    {
        private static LocalDataHelper localDataHelper = new LocalDataHelper();
       
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
            if (!localDataHelper.KeyExists(LocalDataHelper.LOCATION_MSG) || localDataHelper.GetValue<bool>(LocalDataHelper.LOCATION_MSG) == false)
            {
                localDataHelper.SetValue(LocalDataHelper.LOCATION_MSG, true); 
                Show(App.ResLoader.GetString("TurnOnLocationInstruction"), App.ResLoader.GetString("NoLocation"));
            }
        }
    }
}
