using System;
using System.Collections.Generic;
using PropertyChanged;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Data;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.Repository;
using VirtualGuide.Mobile.BindingModel;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using VirtualGuide.Mobile.Common;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace VirtualGuide.Mobile.ViewModel.GuideMain
{

    public class GuideMainViewModel : BaseViewModel
    {
        #region events

        public event Action DataLoaded;

        public event Action<GuideMainPropertyBindingModel> ScrollRequested;

        #endregion

        #region constructors
        public GuideMainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Properties = new ObservableCollection<GuideMainPropertyBindingModel>();
            Travel = new GuideMainBindingModel();

            Initialize();
        }

        #endregion

        #region commands

        public RelayCommand NavigateToMapCommand { get; set; }

        public RelayCommand<GuideMainPropertyBindingModel> PropertyClickCommand { get; set; }

        #endregion

        #region public properties

        private GuideMainBindingModel _travel;
        public GuideMainBindingModel Travel
        {
            get { return _travel; }
            private set { Set(ref _travel, value); }
        }

        private ImageSource _mapImage;
        public ImageSource MapImage
        { 
            get
            {
                if(Travel != null && !String.IsNullOrEmpty(Travel.Name))
                {
                    return new BitmapImage(new Uri(String.Format("ms-appdata:///local/maps/{0}map.png", Travel.Name)));
                }

                return null;
            }
            private set { Set(ref _mapImage, value); }
        }

        private ObservableCollection<GuideMainPropertyBindingModel> _properties;
        public ObservableCollection<GuideMainPropertyBindingModel> Properties
        {
            get { return _properties; }
            private set { Set(ref _properties, value); }
        }

        #endregion

        #region private properties

        private int _travelId;
        private PropertyRepository _propertyRepository = new PropertyRepository();
        private TravelRepository _travelRepository = new TravelRepository();

        #endregion

        #region private methods

        private void Initialize()
        {
            NavigateToMapCommand = new RelayCommand(NavigateToMapExecute);
            PropertyClickCommand = new RelayCommand<GuideMainPropertyBindingModel>(PropertyClickExecute);

            //Move it somewhere else anyway
            //var tours = new GuideMainPropertyBindingModel() {
            //    Name = "Tours", 
            //    Background=VirtualGuide.Mobile.Helper.ColorHelper.GREEN, 
            //    Icon="\uD83C\uDFF0", 
            //    Type=GuideMainPropertyBindingModel.Types.TOURS};

            //Properties.Add(tours);
        }

        #endregion

        #region public methods

        public async void LoadData()
        {
            IsWorkInProgress = true;
            Properties.Clear();

            if (_travelId != 0 || (Travel != null && Travel.Id != 0))
            {
                Travel = await _travelRepository.GetTravelByIdAsync<GuideMainBindingModel>(_travelId);
             
                var props = (await _propertyRepository.GetSimplePropertiesWithColors(_travelId));
                
                foreach(var prop in props)
                {
                    Properties.Add(prop);
                }

            }

            if (DataLoaded != null)
            {
                DataLoaded();
            }
            IsWorkInProgress = false;
        }

        public void NavigateToMapExecute()
        {
            _navigationService.NavigateTo("Maps", _travelId);
        }

        public void PropertyClickExecute(GuideMainPropertyBindingModel item)
        {

            switch(item.Type)
            {
                case GuideMainPropertyBindingModel.Types.TOURS:
                    //todo
                break;
                case GuideMainPropertyBindingModel.Types.REGULAR:
                    if (ScrollRequested != null)
                    {
                        ScrollRequested(item);
                    }
                    
                break;
            }
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
