using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Net.Http;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;
using VirtualGuide.Mobile.Service;
using Windows.UI.Popups;

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
        private TravelService _travelService = new TravelService();
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
                MessageBoxHelper.Show(App.ResLoader.GetString("DownloadMaps"));
                IsWorkInProgress = true;

                var travel = await _travelService.DownloadBoughtTravel(_travelId);

                _navigationService.NavigateTo("GuideList", travel);
            }
            catch (TravelAlreadyOwnedException)
            {
                MessageBoxHelper.Show(App.ResLoader.GetString("TravelAlreadyOwned"));
                _navigationService.NavigateTo("GuideList");
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

            if (!UserService.IsUserLoggedIn())
            {
                MessageBoxHelper.ShowWithCommands(App.ResLoader.GetString("NotLoggedInGuideDownload"), "", new List<UICommand>() {
                    new UICommand(App.ResLoader.GetString("Yes"), new UICommandInvokedHandler(this.GoToLoginCommandInvokedHandler)),
                    new UICommand(App.ResLoader.GetString("Skip")),
                });
            }

            base.NavigationHelper_LoadState(sender, e);
        }

        private void GoToLoginCommandInvokedHandler(IUICommand command)
        {
            _navigationService.NavigateTo("Login");
        }

        #endregion
    }
}
