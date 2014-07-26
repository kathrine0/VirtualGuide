using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Repository;
using VirtualGuide.Mobile.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private UserRepository _userRepository = new UserRepository();
        private bool _loginInProgress = false;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (_loginInProgress) return;

            _loginInProgress = true;

            var username = Username.Text;
            var password = Password.Password;
            var buttonContent = LoginButton.Content;
            var error = false;

            if (username == string.Empty || password == string.Empty)
            {
                MessageBoxHelper.Show("Enter username and password", "");
                error = true;
            }

            try
            {
                LoginButton.Content = "";
                LoginProgress.Visibility = Visibility.Visible;
                this.SpinningAnimation.Begin();
                await _userRepository.Login(username, password);
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == System.Net.HttpStatusCode.NotFound.ToString())
                {
                    MessageBoxHelper.Show("No internet connection. Turn on Data Transfer or Wifi or skip login and work offline.", "No Connection");
                }
                else if (ex.Message == System.Net.HttpStatusCode.BadRequest.ToString())
                {
                    MessageBoxHelper.Show("Invalid username or password", "Error");
                }
                else
                {
                    MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");
                }

                error = true;
            }
            catch
            {
                MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");
                error = true;
            }

            if (!error)
            {
                Frame.Navigate(typeof(GuideList));
            }
            else
            {
                LoginButton.Content = buttonContent;
                LoginProgress.Visibility = Visibility.Collapsed;
            }

            _loginInProgress = false;
        }

        private void SkipLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(GuideList));
        }
    }
}
