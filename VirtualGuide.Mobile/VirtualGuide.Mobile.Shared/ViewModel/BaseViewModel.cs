using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.View;
using VirtualGuide.Mobile.ViewModel.Interfaces;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VirtualGuide.Mobile.ViewModel
{
    public class BaseViewModel : ViewModelBase, INavigableViewModel
    {

        #region private properties

        protected INavigationService _navigationService;

        #endregion

        #region constructors

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OnNavigatedToCommand = new RelayCommand<NavigationEventArgs>(OnNavigatedTo);
            OnNavigatedFromCommand = new RelayCommand<NavigationEventArgs>(OnNavigatedFrom);
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

        private string _defaultLoadingText = App.ResLoader.GetString("Loading");
        private string _progressText = string.Empty;

        public string ProgressText
        {
            get 
            { 
                if (!String.IsNullOrEmpty(_progressText))
                {
                    return _progressText; 
                }
                else
                {
                    return _defaultLoadingText;
                }
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
            statusBar.ForegroundColor = ((SolidColorBrush)ColorHelper.GRAY).Color;
            

            if (IsWorkInProgress)
            {
                statusBar.ProgressIndicator.Text = ProgressText;
                await statusBar.ProgressIndicator.ShowAsync();
            }
            else
            {
                ProgressText = string.Empty;
                //statusBar.ProgressIndicator.Text = string.Empty;
                await statusBar.ProgressIndicator.HideAsync();
            }
#endif
        }

        #endregion

        #region INavigableViewModel

        public RelayCommand<NavigationEventArgs> OnNavigatedToCommand
        {
            get;
            set;
        }

        public RelayCommand<NavigationEventArgs> OnNavigatedFromCommand
        {
            get;
            set;
        }

        protected virtual void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected virtual void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        public NavigationHelper NavigationHelper
        {
            get;
            private set;
        }

        public virtual void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        public virtual void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        public void SetNavigationHelper(NavigationHelper nh)
        {
            NavigationHelper = nh;
            NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            NavigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        #endregion


    }
}
