﻿using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;
using Windows.UI.Xaml.Data;
using VirtualGuide.Mobile.BindingModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Views;

namespace VirtualGuide.Mobile.ViewModel.GuideList
{

    public class GuideListViewModel : BaseViewModel
    {
        #region private properties

        private TravelRepository _travelRepository = new TravelRepository();
        private UserRepository _userRepository = new UserRepository();
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
            set { Set(ref _data, value); }
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

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set { Set(ref _loading, value); }
        }

        #endregion

        #region public methods

        public void TravelItemClickExecute(GuideListBindingModel item)
        {
            if (item.IsOwned)
            {
                _navigationService.NavigateTo("GuideMain", item.Id);
            }
            else if (!item.IsOwned)
            {
                //TODO
                //App.RootFrame.Navigate(_storePage, item.Id);
            }
        }

        
        public async void RefreshExecute()
        {
            //var data = new List<GuideListBindingModel>();
            Data = new ObservableCollection<GuideListBindingModel>();
            var Download = new List<Task>();
            
            if (localDataHelper.GetValue<bool>(LocalDataHelper.LOAD_IN_PROGRESS)) return;
            localDataHelper.SetValue(LocalDataHelper.LOAD_IN_PROGRESS, true);

            try
            {
                Loading = true;

                Download.Add(StartDownloading(_travelRepository.DownloadAvailableTravels()));
                Download.Add(StartDownloading(_travelRepository.DownloadOwnedTravels()));

                await Task.WhenAll(Download);

            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == System.Net.HttpStatusCode.NotFound.ToString())
                {
                    MessageBoxHelper.Show("Please check your internet connection.", "No Connection");
                }
                else if (ex.Message == System.Net.HttpStatusCode.Unauthorized.ToString())
                {
                    MessageBoxHelper.Show("Please log in using your login and password.", "No identity");

                    _navigationService.NavigateTo("Login");
                }
                else
                {
                    MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");
                }
            }
            finally
            {
                localDataHelper.SetValue(LocalDataHelper.LOAD_IN_PROGRESS, false);

                Loading = false;
            }
        }

        public async void LogoutExecute()
        {
            await _userRepository.Logout();

            _navigationService.NavigateTo("Login");
        }

        #endregion

        #region private methods

        private async void Initialize()
        {
            TravelItemClickCommand = new RelayCommand<GuideListBindingModel>(TravelItemClickExecute);
            RefreshCommand = new RelayCommand(RefreshExecute);
            LogoutCommand = new RelayCommand(LogoutExecute);

            Loading = true;
            //TODO Check if token is still active
            Data = new ObservableCollection<GuideListBindingModel>(new List<GuideListBindingModel>(await _travelRepository.GetAllTravelsAsync()));

            //check if just logged in or if data is empty
            var shouldReload = localDataHelper.GetValue<bool>(LocalDataHelper.REFRESH_NOW);
            if (Data.Count == 0 || shouldReload)
            {
                RefreshExecute();
                localDataHelper.SetValue(LocalDataHelper.REFRESH_NOW, false);
            }

            if (!localDataHelper.GetValue<bool>(LocalDataHelper.LOAD_IN_PROGRESS))
            {
                Loading = false;
            }
        }
        private async Task StartDownloading(Task<List<GuideListBindingModel>> task)
        {
            var data = await task;
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                foreach (var item in data)
                {
                    Data.Add(item);
                }

                this.RaisePropertyChanged("DataGrouped");
            });
        }

        #endregion
    }
}

