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
using Microsoft.Practices.Prism.Commands;
using Windows.UI.Xaml.Controls;

namespace VirtualGuide.Mobile.ViewModel.GuideMain
{

    [ImplementPropertyChanged]
    public class GuideMainViewModel
    {
        #region readonly properties

        private readonly Type _mapsPage;

        #endregion

        #region events

        public event Action DataLoaded;

        public event Action<GuideMainPropertyBindingModel> ScrollRequested;

        #endregion

        #region constructors
        public GuideMainViewModel(Type mapsPage)
        {

            Travel = new GuideMainBindingModel();

            Initialize();
            _mapsPage = mapsPage;
        }
        public GuideMainViewModel(Travel travel, Type mapsPage)
            : this(mapsPage)
        {
            Travel = new GuideMainBindingModel(travel);
        }


        #endregion

        #region commands

        public DelegateCommand NavigateToMapCommand { get; set; }

        public DelegateCommand<GuideMainPropertyBindingModel> PropertyClickCommand { get; set; }

        #endregion

        #region public properties

        public GuideMainBindingModel Travel
        {
            get;
            private set;
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
            private set
            {
                _mapImage = value;
            }
        }

        public ObservableCollection<GuideMainPropertyBindingModel> Properties
        {
            get;
            private set;
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
            NavigateToMapCommand = new DelegateCommand(NavigateToMapExecute);
            PropertyClickCommand = new DelegateCommand<GuideMainPropertyBindingModel>(PropertyClickExecute);

            var tours = new GuideMainPropertyBindingModel() {
                Name = "Tours", 
                Background=VirtualGuide.Mobile.Helper.ColorHelper.GREEN, 
                Symbol="\uD83C\uDFF0", 
                Type=GuideMainPropertyBindingModel.Types.TOURS};

            Properties = new ObservableCollection<GuideMainPropertyBindingModel>();
            Properties.Add(tours);
        }

        #endregion

        #region public methods

        public void LoadData(int travelId)
        {
            _travelId = travelId;
            LoadData();
        }

        public async void LoadData()
        {
            if (_travelId != 0 || (Travel != null && Travel.Id != 0))
            {
                Travel = await _travelRepository.GetTravelByIdAsync<GuideMainBindingModel>(_travelId);
             
                var props = (await _propertyRepository.GetSimpleProperties<GuideMainPropertyBindingModel>(_travelId));
                
                foreach(var prop in props)
                {
                    Properties.Add(prop);
                }                   
            }

            if (DataLoaded != null)
            {
                DataLoaded();
            }
        }

        public void NavigateToMapExecute()
        {
            App.RootFrame.Navigate(_mapsPage, Travel.Id);
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

    }

    
    
}
