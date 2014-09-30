using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;

namespace VirtualGuide.Mobile.ViewModel.RegisterPage
{
    [ImplementPropertyChanged]
    class RegisterViewModel
    {
        #region readonly properties

        private readonly Type _nextPageType;

        #endregion

        #region constructors

        public RegisterViewModel(Type nextPageType)
        {
            RegisterCommand = new DelegateCommand(RegisterExecute);

            RegisterButtonContent = "Register";

            _nextPageType = nextPageType;
        }

        #endregion

        #region events
        #endregion

        #region private properties

        private UserRepository _userRepository = new UserRepository();

        private bool _registerInProgress = false;

        private LocalDataHelper localDataHelper = new LocalDataHelper();

        #endregion

        #region public properties

        public string Username { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public string RegisterButtonContent { get; set; }

        public bool RegistrationInProgress { get; set; }

        public bool ShowLoader { get; set; }

        #endregion

        #region commands

        /// <summary>
        /// Validate form and Register user
        /// </summary>
        public DelegateCommand RegisterCommand
        {
            get;
            set;
        }

        #endregion

        #region public methods

        private async void RegisterExecute()
        {
            if (_registerInProgress)
            {
                return;
            }

            RegisterButtonContent = "";
            ShowLoader = true;

            if (string.IsNullOrEmpty(Username) || 
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(RepeatPassword))
            {
                MessageBoxHelper.Show("Enter username and passwords", "");
                TurnOffLoader();

                return;
            }

            //TODO! Perform Register and login
            //try
            //{
            //    await _userRepository.Login(Username, Password);

            //}
            //catch (HttpRequestException ex)
            //{
            //}

            if (_nextPageType != null)
            {
                localDataHelper.SetValue(LocalDataHelper.REFRESH_NOW, true);
                App.RootFrame.Navigate(_nextPageType);
            }

        }

        #endregion

        #region private methods

        private void TurnOffLoader()
        {
            ShowLoader = false;
            RegisterButtonContent = "Register";
        }

        #endregion
    }
}
