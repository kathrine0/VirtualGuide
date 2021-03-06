﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;
using VirtualGuide.Mobile.Service;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;

namespace VirtualGuide.Mobile.ViewModel.GuideList
{

    public class GuideListViewModel : BaseViewModel
    {
        #region private properties

        private TravelRepository _travelRepository = new TravelRepository();
        private TravelService _travelService = new TravelService();
        private UserService _userRepository = new UserService();
        LocalDataHelper localDataHelper = new LocalDataHelper();

        #endregion

        #region constructors

        public GuideListViewModel(INavigationService navigationService) : base(navigationService)
        {           
            Initialize();
        }

        #endregion

        #region commands

        public RelayCommand<GuideListBindingModel> TravelItemClickCommand { get; set; }

        public RelayCommand RefreshCommand { get; set; }

        public RelayCommand LogoutCommand { get; set; }


        #endregion

        #region events

        #endregion
        
        #region public properties

        private ObservableCollection<GuideListBindingModel> _data = new ObservableCollection<GuideListBindingModel>();

        public ObservableCollection<GuideListBindingModel> Data
        {
            get { return _data; }
            private set { Set(ref _data, value); }
        }

        public ObservableCollection<ListGroup<GuideListBindingModel>> DataGrouped
        {
            get
            {
                if (Data != null)
                {
                    var grouped = Data.ToObservableGroups(x => x.Name, x => x.IsOwned, true);

                    return grouped;
                }
                return null;
            }
        }

        #endregion

        #region public methods
        #endregion

        #region private methods
        private void TravelItemClickExecute(GuideListBindingModel item)
        {
            if (item.IsOwned)
            {
                _navigationService.NavigateTo("GuideMain", item.Id);
            }
            else if (!item.IsOwned)
            {
                //if (Data.FirstOrDefault(x => x.Id == x.))
                _navigationService.NavigateTo("BuyGuide", item.Id);
            }
        }

        private async void RefreshExecute()
        {
            var Download = new List<Task>();

            if (IsWorkInProgress) return;
            
            try
            {
                IsWorkInProgress = true;
                ProgressText = App.ResLoader.GetString("Downloading");

                Download.Add(StartDownloading(_travelService.DownloadAvailableTravels()));
                Download.Add(StartDownloading(_travelService.DownloadOwnedTravels()));

                await Task.WhenAll(Download);
                


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

                RestoreOldGuides();
            }
            finally
            {
                IsWorkInProgress = false;
            }
        }

        private async void LogoutExecute()
        {
            await _userRepository.Logout();

            _navigationService.NavigateTo("Login");
        }

        private async void Initialize()
        {
            TravelItemClickCommand = new RelayCommand<GuideListBindingModel>(TravelItemClickExecute);
            RefreshCommand = new RelayCommand(RefreshExecute);
            LogoutCommand = new RelayCommand(LogoutExecute);

            IsWorkInProgress = true;
            //TODO Check if token is still active
            Data = new ObservableCollection<GuideListBindingModel>(new List<GuideListBindingModel>(await _travelRepository.GetAllTravelsAsync()));

            IsWorkInProgress = false;

            //check if just logged in or if data is empty
            var shouldReload = localDataHelper.GetValue<bool>(LocalDataHelper.REFRESH_NOW);
            if (Data.Count == 0 || shouldReload)
            {
                RefreshExecute();
                localDataHelper.SetValue(LocalDataHelper.REFRESH_NOW, false);
            }

        }

        private async Task StartDownloading(Task<List<GuideListBindingModel>> task)
        {
            var data = await task;
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                foreach (var item in data)
                {
                    InsertTravelToData(item);
                }
            });
            this.RaisePropertyChanged("DataGrouped");
        }

        private void InsertTravelToData(GuideListBindingModel item)
        {
            var duplicate = Data.FirstOrDefault(i => i.Id == item.Id);

            //if owned item already exists continue
            if (duplicate != null && !item.IsOwned && duplicate.IsOwned)
            {
                return;
            }
            //if new item is owned overide or if both items are not owned
            if (duplicate != null && item.IsOwned ||
                duplicate != null && !item.IsOwned && !duplicate.IsOwned)
            {
                Data.Remove(duplicate);
            }

            Data.Add(item);
        }

        private async void RestoreOldGuides()
        {
            IsWorkInProgress = true;
            Data = new ObservableCollection<GuideListBindingModel>(new List<GuideListBindingModel>(await _travelRepository.GetAllTravelsAsync()));
            IsWorkInProgress = false;
        }

        #endregion

        #region navigation

        private void HardwareButtonsBackPressedExecute(object sender, BackPressedEventArgs e)
        {
            //Quit application when no authentification is required
            SettingsDataHelper settingsDataHelper = new SettingsDataHelper();
            if (!String.IsNullOrEmpty(settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN)))
            {
                App.Current.Exit();
            }
        }

        public override void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtonsBackPressedExecute;

            if (e.NavigationParameter != null && e.NavigationParameter is GuideListBindingModel)
            {
                var newTravel = e.NavigationParameter as GuideListBindingModel;
                if (!Data.Contains(newTravel))
                {
                    InsertTravelToData(newTravel);
                    this.RaisePropertyChanged("DataGrouped");
                }
            }

            base.NavigationHelper_LoadState(sender, e);
        }

        public override void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtonsBackPressedExecute;

            base.NavigationHelper_SaveState(sender, e);
        }

        #endregion

    }
}

