using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;

namespace VirtualGuide.Mobile.ViewModel
{
    [ImplementPropertyChanged]
    public class LoginViewModel
    {
        private UserRepository _userRepository = new UserRepository();

        public LoginViewModel()
        {
            SignInCommand = new DelegateCommand(SignInExecute);
            SkipLoginCommand = new DelegateCommand(SkipLoginExecute);
        }

        #region events

        /// <summary>
        /// Occurs after successful signing into the system
        /// </summary>
        public event Action SignInSuccessful;

        /// <summary>
        /// Occurs when Signing in started
        /// </summary>
        public event Action SignInInProgress;

        /// <summary>
        /// Occurs when Singning in failed
        /// </summary>
        public event Action SignInFailed;

        /// <summary>
        /// Occurs when User skips login
        /// </summary>
        public event Action SkipLogin;

        #endregion

        #region properties

        public string Username { get; set; }
        public string Password { get; set; }

        private bool _loginInProgress = false;

        public bool LoginInProgress
        {
            get { return _loginInProgress = false; }
            set { _loginInProgress = value; }
        }
        
        #endregion

        #region commands

        public DelegateCommand SignInCommand
        {
            get;
            set;
        }

        public DelegateCommand SkipLoginCommand
        {
            get;
            set;
        }

        #endregion

        #region methods

        private async void SignInExecute()
        {
            if (_loginInProgress)
            {
                return;
            }

            if (SignInInProgress != null)
            {
                SignInInProgress();
            }


            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBoxHelper.Show("Enter username and password", "");
                TriggerSignInFailureEvent();

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
                TriggerSignInFailureEvent();

                return;
            }
            catch
            {
                MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");
                TriggerSignInFailureEvent();

                return;
            }

            if (SignInSuccessful != null)
            {
                SignInSuccessful();
            }
            
        }

        private async void SkipLoginExecute()
        {
            if (SkipLogin != null)
            {
                SkipLogin();
            }
        }

        #endregion

        #region private

        private void TriggerSignInFailureEvent()
        {
            if (SignInFailed != null)
            {
                SignInFailed();
            }
        }

        #endregion
    }
}
