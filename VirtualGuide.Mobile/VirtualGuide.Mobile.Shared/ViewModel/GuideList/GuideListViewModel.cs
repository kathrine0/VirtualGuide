using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
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

namespace VirtualGuide.Mobile.ViewModel.GuideList
{

    //[ImplementPropertyChanged]
    public class GuideListViewModel : INotifyPropertyChanged
    {
        #region readonly properties

        private readonly Type _loginPage;
        private readonly Type _guideMainPage;


        #endregion

        #region constructors

        public GuideListViewModel(Type loginPage, Type guideMainPage)
        {
            _loginPage = loginPage;
            _guideMainPage = guideMainPage;

            Initialize();
        }

        #endregion

        #region commands

        public DelegateCommand<GuideListBindingModel> TravelItemClickCommand { get; set; }

        public DelegateCommand RefreshCommand { get; set; }

        public DelegateCommand LogoutCommand { get; set; }

        #endregion

        #region events

        #endregion

        #region private properties

        private TravelRepository _travelRepository = new TravelRepository();
        private UserRepository _userRepository = new UserRepository();
        LocalDataHelper localDataHelper = new LocalDataHelper();

        #endregion
        
        #region public properties

        private ObservableCollection<GuideListBindingModel> _data = new ObservableCollection<GuideListBindingModel>();
        [AlsoNotifyFor("Collection")]
        public ObservableCollection<GuideListBindingModel> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region public methods

        public void TravelItemClickExecute(GuideListBindingModel item)
        {
            if (item.IsOwned && _guideMainPage != null)
            {
                App.RootFrame.Navigate(_guideMainPage, item.Id);
            }
            else if (!item.IsOwned)
            {
                //TODO
                //App.RootFrame.Navigate(_storePage, item.Id);
            }
        }

        public async void RefreshExecute()
        {
            var data = new List<GuideListBindingModel>();
            var Download = new List<Task<List<GuideListBindingModel>>>();
            
            if (localDataHelper.GetValue<bool>(LocalDataHelper.LOAD_IN_PROGRESS)) return;
            localDataHelper.SetValue(LocalDataHelper.LOAD_IN_PROGRESS, true);

            try
            {
                Loading = true;

                Download.Add(_travelRepository.DownloadAvailableTravels());
                Download.Add(_travelRepository.DownloadOwnedTravels());

                foreach (var d in Download)
                {
                    //d.ContinueWith(t =>
                    //{
                    //    foreach (var item in t.Result)
                    //        Data.Add(item);
                    //});

                    var result = await d;

                    foreach (var item in result)
                        data.Add(item);
                    
                }

                //change me
                Data = new ObservableCollection<GuideListBindingModel>(data);
                
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

                    if (_loginPage != null)
                    {
                        App.RootFrame.Navigate(_loginPage);
                    }
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

            if (_loginPage != null)
                App.RootFrame.Navigate(_loginPage);
        }

        #endregion

        #region private methods

        private async void Initialize()
        {
            TravelItemClickCommand = new DelegateCommand<GuideListBindingModel>(TravelItemClickExecute);
            RefreshCommand = new DelegateCommand(RefreshExecute);
            LogoutCommand = new DelegateCommand(LogoutExecute);

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

        #endregion
    }
}

