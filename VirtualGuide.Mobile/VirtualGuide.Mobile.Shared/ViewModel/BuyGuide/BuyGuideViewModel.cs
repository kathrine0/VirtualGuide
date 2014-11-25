using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;

namespace VirtualGuide.Mobile.ViewModel
{
    class BuyGuideViewModel : BaseViewModel
    {
        #region constructors

        public BuyGuideViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Travel = new GuideListBindingModel();
        }

        #endregion

        #region private properties

        private int _travelId;
        private TravelRepository _travelRepository = new TravelRepository();

        #endregion

        #region public properties

        private GuideListBindingModel _travel;
        public GuideListBindingModel Travel 
        {
            get { return _travel; }
            private set { Set(ref _travel, value); }
        }

        #endregion

        #region private methods

        private async void LoadData()
        {
            IsWorkInProgress = true;

            if (_travelId != 0)
            {
                Travel = await _travelRepository.GetTravelByIdAsync<GuideListBindingModel>(_travelId);
            }

            IsWorkInProgress = false;
        }

        #endregion

        #region navigation

        public override void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _travelId = (int)e.NavigationParameter;
            LoadData();

            base.NavigationHelper_LoadState(sender, e);
        }

        #endregion
    }
}
