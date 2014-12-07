using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.Repository;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.ViewModel.GuideMain
{

    public class GuideMainViewModel : BaseViewModel
    {
        #region events

        public event Action DataLoaded;

        public event Action<PropertyBindingModel> ScrollRequested;

        #endregion

        #region constructors
        public GuideMainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Properties = new ObservableCollection<PropertyBindingModel>();
            Travel = new GuideMainBindingModel();

            Initialize();
        }

        #endregion

        #region commands

        public RelayCommand NavigateToMapCommand { get; set; }

        public RelayCommand<PropertyBindingModel> PropertyClickCommand { get; set; }

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

        private ObservableCollection<PropertyBindingModel> _properties;
        public ObservableCollection<PropertyBindingModel> Properties
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
            PropertyClickCommand = new RelayCommand<PropertyBindingModel>(PropertyClickExecute);

            //Move it somewhere else anyway
            //var tours = new PropertyBindingModel() {
            //    Name = "Tours", 
            //    Background=VirtualGuide.Mobile.Helper.ColorHelper.GREEN, 
            //    Icon="\uD83C\uDFF0", 
            //    Type=PropertyBindingModel.Types.TOURS};

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
             
                var props = (await _propertyRepository.GetSimpleProperties(_travelId));
                
                List<Brush> colors = new List<Brush>() { ColorHelper.BLUE, ColorHelper.GREEN, ColorHelper.RED };

                for (var i = 0; i < props.Count; i++ )
                {
                    props[i].Background = colors[i % colors.Count];
                    Properties.Add(props[i]);
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

        public void PropertyClickExecute(PropertyBindingModel item)
        {

            switch(item.Type)
            {
                case PropertyBindingModel.Types.TOURS:
                    //todo
                break;
                case PropertyBindingModel.Types.REGULAR:
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
