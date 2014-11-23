using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
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
    public class LoginViewModel : BaseViewModel
    {
        #region constructors

        /// <summary>
        /// ViewModel for Login Page
        /// </summary>
        /// <param name="guideListType">Type of page to navigate to after login</param>
        public LoginViewModel(INavigationService navigationService)
            : base(navigationService)
        {            
            SignInCommand = new RelayCommand(SignInExecute);
            SkipLoginCommand = new RelayCommand(SkipLoginExecute);
            RegisterCommand = new RelayCommand(RegisterExecute);

            TurnOffLoader();
        }

        #endregion

        #region events

        #endregion

        #region private properties

        private UserRepository _userRepository = new UserRepository();
        private LocalDataHelper localDataHelper = new LocalDataHelper();

        #endregion
        
        #region public propeties

        public string Email { get; set; }
      
        public string Password { get; set; }

        public string LoginButtonContent { get; set; }

        public bool LoginInProgress { get; set; }
        
        #endregion

        #region commands

        /// <summary>
        /// Check Email and Password and Log User in
        /// </summary>
        public RelayCommand SignInCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Navigate to Register page
        /// </summary>
        public RelayCommand RegisterCommand { get; set; }

        /// <summary>
        /// Skip login and redirect to next page
        /// </summary>
        public RelayCommand SkipLoginCommand
        {
            get;
            set;
        }

        #endregion

        #region private methods

        private async void SignInExecute()
        {
            if (LoginInProgress)
            {
                return;
            }

            LoginButtonContent = "";
            LoginInProgress = true;
            IsWorkInProgress = true;

            var success = false;

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                MessageBoxHelper.Show("Enter username and password", "");
                TurnOffLoader();

                return;
            }

            try
            {
                success = await _userRepository.Login(Email, Password);

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
            }
            catch
            {
                MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");
            }

            TurnOffLoader();

            if (success)
            {
                localDataHelper.SetValue(LocalDataHelper.REFRESH_NOW, true);
                _navigationService.NavigateTo("GuideList");
            }
            

        }

        private void SkipLoginExecute()
        {
            _navigationService.NavigateTo("GuideList");
        }

        private void RegisterExecute()
        {
            _navigationService.NavigateTo("Register");
        }

        #endregion

        #region public methods

        public void TurnOffLoader()
        {
            LoginInProgress = false;
            LoginButtonContent = "Login";
        }

        #endregion
    }
}
