using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.DBModel;
using VirtualGuide.Mobile.Repository;
using Windows.UI.Xaml.Data;
using VirtualGuide.Mobile.Model;

namespace VirtualGuide.Mobile.ViewModel.GuideList
{

    [ImplementPropertyChanged]
    public class GuideListViewModel
    {
        #region constructors

        public GuideListViewModel()
        {
            Initialize();
        }

        public GuideListViewModel(Travel travel) : this()
        {
        }

        #endregion

        #region commands

        public DelegateCommand<TravelModel> TravelItemClickCommand { get; set; }

        public DelegateCommand RefreshCommand { get; set; }

        public DelegateCommand LogoutCommand { get; set; }

        #endregion

        #region events

        public event Action<int> OwnedItemClicked;

        public event Action<int> NotOwnedItemClicked;

        public event Action RefreshFailedNoIdentity;

        public event Action LogoutSuccessful;

        #endregion

        #region properties

        private TravelRepository _travelRepository = new TravelRepository();

        public List<TravelModel> Data
        {
            get;
            set;
        }

        private CollectionViewSource _collection;
        public CollectionViewSource Collection
        {
            get
            {
                _collection = new CollectionViewSource();
                if (Data != null)
                {
                    var grouped = Data.ToGroups(x => x.Name, x => x.IsOwned, true);
                    _collection.Source = grouped;
                    _collection.IsSourceGrouped = true;
                }
                return _collection;
            }
        }

        public bool Loading { get; set; }

        #endregion

        #region public methods

        public void TravelItemClickExecute(TravelModel item)
        {
            if (item.IsOwned && OwnedItemClicked != null)
            {
                OwnedItemClicked(item.Id);
            }
            else if (!item.IsOwned && NotOwnedItemClicked != null)
            {
                NotOwnedItemClicked(item.Id);
            }
        }

        public async void RefreshExecute()
        {
            if (LocalDataHelper.GetKeyValue<bool>("RefreshInProgress")) return;
            LocalDataHelper.SetValue("RefreshInProgress", true);

            try
            {
                Loading = true;

                Data = await _travelRepository.DownloadAndSaveAllTravels();
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

                    if (RefreshFailedNoIdentity != null)
                    {
                        RefreshFailedNoIdentity();
                    }

                }
                else
                {
                    MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");

                }
            }
            finally
            {
                LocalDataHelper.SetValue("RefreshInProgress", false);

                Loading = false;
            }
        }

        public async void GetData()
        {
            Loading = true;
            //TODO Check if token is still active
            Data = new List<TravelModel>(await _travelRepository.GetAllTravelsAsync());

            Loading = false;
        }

        public void LogoutExecute()
        {
            //TODO

            if (LogoutSuccessful != null)
            {
                LogoutSuccessful();
            }
        }

        #endregion

        #region private methods

        private void Initialize()
        {
            TravelItemClickCommand = new DelegateCommand<TravelModel>(TravelItemClickExecute);
            RefreshCommand = new DelegateCommand(RefreshExecute);
            LogoutCommand = new DelegateCommand(LogoutExecute);

            Loading = false;
        }

        #endregion
    }
}

