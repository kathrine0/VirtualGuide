using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

            Initialize();
        }

        #endregion

        #region commands

        public RelayCommand BuyCommand { get; set; }

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

        private void Initialize()
        {
            BuyCommand = new RelayCommand(BuyExecute);
        }

        private async void BuyExecute()
        {
            if (IsWorkInProgress) return;

            try
            {
                IsWorkInProgress = true;

                var travel = await _travelRepository.DownloadBoughtTravel(_travelId);
                IsWorkInProgress = false;
                _navigationService.NavigateTo("GuideList", travel);
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == System.Net.HttpStatusCode.NotFound.ToString())
                {
                    MessageBoxHelper.Show(App.ResLoader.GetString("TurnOnTransfer"), App.ResLoader.GetString("NoConnection"));
                }
                else if (ex.Message == System.Net.HttpStatusCode.Unauthorized.ToString())
                {
                    MessageBoxHelper.Show(App.ResLoader.GetString("PleaseLogIn"), App.ResLoader.GetString("NoIdentity"));

                    _navigationService.NavigateTo("Login");
                }
                else
                {
                    MessageBoxHelper.Show(App.ResLoader.GetString("UnexpectedError"), App.ResLoader.GetString("Error"));
                }
            }
            finally
            {
                IsWorkInProgress = false;
            }

        }

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
