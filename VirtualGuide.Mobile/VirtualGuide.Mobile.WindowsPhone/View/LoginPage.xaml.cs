using VirtualGuide.Mobile.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private LoginViewModel _loginViewModel = new LoginViewModel();

        public LoginPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            LoginViewModel.SignInSuccessful += NavigateToNextPage;
            LoginViewModel.SignInInProgress += ShowLoader;
            LoginViewModel.SignInFailed += HideLoader;
            LoginViewModel.SkipLogin += NavigateToNextPage;
        }

        public LoginViewModel LoginViewModel
        {
            get { return this._loginViewModel; }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void NavigateToNextPage()
        {
            Frame.Navigate(typeof(GuideList));
        }

        private void ShowLoader()
        {
            LoginButton.Content = "";
            LoginProgress.Visibility = Visibility.Visible;
            this.SpinningAnimation.Begin();
        }

        private void HideLoader()
        {
            LoginButton.Content = "Login";
            LoginProgress.Visibility = Visibility.Collapsed;
        }
    }
}
