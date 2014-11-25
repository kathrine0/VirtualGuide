using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.ViewModel.Interfaces;
using VirtualGuide.Mobile.ViewModel.LoginPage;
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
        private INavigableViewModel viewModel;
        
        public LoginPage()
        {
            this.InitializeComponent();

            viewModel = DataContext as INavigableViewModel;
            viewModel.SetNavigationHelper(new NavigationHelper(this));   

            this.SpinningAnimation.Begin();
        }

        #region Navigation

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel.OnNavigatedToCommand.Execute(e);            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            viewModel.OnNavigatedFromCommand.Execute(e);
        }

        #endregion       

    }
}
