using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;
using VirtualGuide.Mobile.View;
using Windows.UI.Xaml.Controls;

namespace VirtualGuide.Mobile.ViewModel.LoginPage
{
    [ImplementPropertyChanged]
    public class LoginViewModel
    {
        #region readonly properties

        private readonly Type _nextPageType;

        #endregion

        #region constructors

        /// <summary>
        /// ViewModel for Login Page
        /// </summary>
        /// <param name="nextPageType">Type of page to navigate to after login</param>
        public LoginViewModel(Type nextPageType)
        {
            SignInCommand = new DelegateCommand(SignInExecute);
            SkipLoginCommand = new DelegateCommand(SkipLoginExecute);

            LoginButtonContent = "Login";

            _nextPageType = nextPageType;
        }

        #endregion

        #region events

        #endregion

        #region properties

        private UserRepository _userRepository = new UserRepository();
       
        private bool _loginInProgress = false;
        
        public string Username { get; set; }
      
        public string Password { get; set; }

        public string LoginButtonContent { get; set; }

        public bool LoginInProgress
        {
            get { return _loginInProgress = false; }
            set { _loginInProgress = value; }
        }

        public bool ShowLoader { get; set; }
        
        #endregion

        #region commands

        /// <summary>
        /// Check Username and Password and Log User in
        /// </summary>
        public DelegateCommand SignInCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Skip login and redirect to next page
        /// </summary>
        public DelegateCommand SkipLoginCommand
        {
            get;
            set;
        }

        #endregion

        #region public methods

        private async void SignInExecute()
        {
            if (_loginInProgress)
            {
                return;
            }

            LoginButtonContent = "";
            ShowLoader = true;


            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBoxHelper.Show("Enter username and password", "");
                TurnOffLoader();

                return;
            }

            try
            {
                await _userRepository.Login(Username, Password);

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
                TurnOffLoader();

                return;
            }
            catch
            {
                MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");
                TurnOffLoader();

                return;
            }

            if (_nextPageType != null)
                App.RootFrame.Navigate(_nextPageType);
        }

        private void SkipLoginExecute()
        {
            if (_nextPageType != null)
                App.RootFrame.Navigate(_nextPageType);
        }

        #endregion

        #region private methods

        private void TurnOffLoader()
        {
            ShowLoader = false;
            LoginButtonContent = "Login";
        }

        #endregion
    }
}
