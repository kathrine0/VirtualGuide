using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtualGuide.Mobile.Model;
using System.Net.Http;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using PropertyChanged;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections;
using Windows.UI.Xaml.Data;
using VirtualGuide.Mobile.Helper;
using Microsoft.Practices.Prism.Commands;
using VirtualGuide.Mobile.Repository;

namespace VirtualGuide.Mobile.ViewModel
{

    [ImplementPropertyChanged]
    public class GuideListViewModel : BaseTravelViewModel
    {
        #region constructors

        public GuideListViewModel() : base()
        {
            Initialize();
        }

        public GuideListViewModel(Travel travel) : base(travel)
        {
            Initialize();
        }

        #endregion

        #region commands

        public DelegateCommand<GuideListViewModel> TravelItemClickCommand { get; set; }

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

        public List<GuideListViewModel> Data
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

        public void TravelItemClickExecute(GuideListViewModel item)
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

                Data = await _travelRepository.DownloadAndSaveAllTravels<GuideListViewModel>();
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
            Data = new List<GuideListViewModel>(await _travelRepository.GetAllTravelsAsync<GuideListViewModel>());

            Loading = false;
        }

        public void LogoutExecute()
        {
            //TODO

            if (LogoutSuccessful !=null)
            {
                LogoutSuccessful();
            }
        }

        #endregion

        #region private methods

        private void Initialize()
        {
            TravelItemClickCommand = new DelegateCommand<GuideListViewModel>(TravelItemClickExecute);
            RefreshCommand = new DelegateCommand(RefreshExecute);
            LogoutCommand = new DelegateCommand(LogoutExecute);

            Loading = false;
        }

        #endregion
    }

    [ImplementPropertyChanged]
    public class GuideMainViewModel : BaseTravelViewModel
    {
        public GuideMainViewModel() : base()
        {

        }

        public GuideMainViewModel(Travel travel)
            : base(travel)
        {
        }

        public List<GuideMainViewModel> Data
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
    }

    [ImplementPropertyChanged]
    public class BaseTravelViewModel
    {
        #region constructors

        public BaseTravelViewModel()
        {

        }

        public BaseTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Price = travel.Price;
            Latitude = travel.Latitude;
            Longitude = travel.Longitude;
            ZoomLevel = travel.ZoomLevel;
            _imageSrc = travel.ImageSrc;
            IsOwned = travel.IsOwned;
        }

        #endregion

        #region properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public List<Property> Properties { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsOwned { get; set; }

        public double ZoomLevel { get; set; }

        private string _imageSrc;

        private ImageSource _imagePath;
        public ImageSource ImagePath
        {
            get
            {
                ImageSource bitmap = null;
                Uri uri = null;
                try
                {
                    uri = new Uri("ms-appdata:///local/images/" + _imageSrc);
                    bitmap = new BitmapImage(uri);
                    _imagePath = bitmap;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                return _imagePath;
            }
        }

        #endregion
    }
}
