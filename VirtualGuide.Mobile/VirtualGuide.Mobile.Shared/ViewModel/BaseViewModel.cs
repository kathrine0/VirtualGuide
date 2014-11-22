using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

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
    }
}
