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

namespace VirtualGuide.Mobile.ViewModel.RegisterPage
{
    public class RegisterViewModel : BaseViewModel
    {
        #region constructors

        public RegisterViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            RegisterCommand = new RelayCommand(RegisterExecute);

            RegisterButtonContent = "Register";
        }

        #endregion

        #region events
        #endregion

        #region private properties

        private UserRepository _userRepository = new UserRepository();
        private LocalDataHelper localDataHelper = new LocalDataHelper();

        #endregion

        #region public properties

        public string Email { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public string RegisterButtonContent { get; set; }

        public bool RegistrationInProgress { get; set; }

        #endregion

        #region commands

        /// <summary>
        /// Validate form and Register user
        /// </summary>
        public RelayCommand RegisterCommand
        {
            get;
            set;
        }

        #endregion

        #region public methods

        private async void RegisterExecute()
        {
            if (RegistrationInProgress)
            {
                return;
            }

            RegisterButtonContent = "";
            RegistrationInProgress = true;

            if (string.IsNullOrEmpty(Email) || 
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(RepeatPassword))
            {
                MessageBoxHelper.Show("Enter username and passwords", "");
                TurnOffLoader();

                return;
            }

            if (string.IsNullOrEmpty(Password) != string.IsNullOrEmpty(RepeatPassword))
            {
                MessageBoxHelper.Show("Passwords don't match", "");
                TurnOffLoader();

                return;
            }

            try
            {
                await _userRepository.Register(Email, Password, RepeatPassword);

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

            //authenticate user
            await _userRepository.Login(Email, Password);

            localDataHelper.SetValue(LocalDataHelper.REFRESH_NOW, true);
            _navigationService.NavigateTo("GuideList");
            
        }

        #endregion

        #region private methods

        private void TurnOffLoader()
        {
            RegistrationInProgress = false;
            RegisterButtonContent = "Register";
        }

        #endregion
    }
}
