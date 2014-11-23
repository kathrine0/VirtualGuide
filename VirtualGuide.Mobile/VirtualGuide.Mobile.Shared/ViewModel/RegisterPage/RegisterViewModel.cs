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

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { Set(ref _repeatPassword, value); }
        }

        private string _registerButtonContent;
        public string RegisterButtonContent
        {
            get { return _registerButtonContent; }
            private set { Set(ref _registerButtonContent, value); }
        }

        private bool _registrationInProgress;
        public bool RegistrationInProgress
        {
            get { return _registrationInProgress; }
            private set { Set(ref _registrationInProgress, value); }
        }

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

            IsWorkInProgress = true;
            RegisterButtonContent = "";
            RegistrationInProgress = true;
            var success = false;

            if (string.IsNullOrEmpty(Email) || 
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(RepeatPassword))
            {
                MessageBoxHelper.Show("Enter username and passwords", "");
            }

            if (string.IsNullOrEmpty(Password) != string.IsNullOrEmpty(RepeatPassword))
            {
                MessageBoxHelper.Show("Passwords don't match", "");
            }

            try
            {
                success = await _userRepository.Register(Email, Password, RepeatPassword);

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
