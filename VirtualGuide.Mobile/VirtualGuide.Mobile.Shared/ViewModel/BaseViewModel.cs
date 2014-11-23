using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Helper;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {

        #region private properties

        protected INavigationService _navigationService;

        #endregion

        #region constructors

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region public properties

        private bool _isWorkInProgress;
        public bool IsWorkInProgress
        {
            get 
            { 
                return _isWorkInProgress; 
            }
            protected set 
            { 
                if (_isWorkInProgress != value)
                {
                    Set(ref _isWorkInProgress, value);
                    OnIsWorkInProgressChanged();
                }
            }
        }

        private string _progressText = "Loading";

        public string ProgressText
        {
            get 
            { 
                return _progressText; 
            }
            protected set
            {
                if (_progressText != value)
                {
                    Set(ref _progressText, value);
                    OnProgressTextChanged();
                }
            }
        }

        #endregion

        #region protected methods

        protected virtual void OnProgressTextChanged()
        {
#if WINDOWS_PHONE_APP
          var statusBar = StatusBar.GetForCurrentView();
          statusBar.ProgressIndicator.Text = ProgressText;
#endif
        }

        protected virtual async void OnIsWorkInProgressChanged()
        {
#if WINDOWS_PHONE_APP
            var statusBar = StatusBar.GetForCurrentView();
            //statusBar.ForegroundColor = ((SolidColorBrush)ColorHelper.BLUE).Color;
            

            if (IsWorkInProgress)
            {
                statusBar.ProgressIndicator.Text = ProgressText;
                await statusBar.ProgressIndicator.ShowAsync();
            }
            else
            {
                statusBar.ProgressIndicator.Text = string.Empty;
                await statusBar.ProgressIndicator.HideAsync();
            }
#endif
        }

        #endregion
    }
}
