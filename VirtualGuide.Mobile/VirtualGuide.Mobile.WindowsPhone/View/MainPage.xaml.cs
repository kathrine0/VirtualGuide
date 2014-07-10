﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using VirtualGuide.Mobile.Common;
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
        private UserViewModel _userViewModel = new UserViewModel();

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
            var username = Username.Text;
            var password = Password.Password;

            try
            {
                await _userViewModel.Login(username, password);
            }
            catch (HttpRequestException)
            {
                MessageBoxHelper.Show("Invalid username or password or no internet connection", "Error");
                return;
            }
            catch
            {
                MessageBoxHelper.Show("Unknown error", "Error");
                return;
            }

            Frame.Navigate(typeof(GuideList));
        }

        private void SkipLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(GuideList));
        }
    }
}