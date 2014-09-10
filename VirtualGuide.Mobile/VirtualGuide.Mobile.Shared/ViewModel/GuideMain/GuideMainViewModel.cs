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

namespace VirtualGuide.Mobile.ViewModel.GuideMain
{

    [ImplementPropertyChanged]
    public class GuideMainViewModel
    {
        #region constructors
        public GuideMainViewModel()
        {
            Travel = new GuideMainBindingModel();
            Initialize();
        }
        public GuideMainViewModel(Travel travel)
        {
            Travel = new GuideMainBindingModel(travel);
            Initialize();
        }


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
                var props = (await _propertyRepository.GetSimpleProperties<GuideMainPropertyBindingModel>(_travelId));
                
                foreach(var prop in props)
                {
                    Properties.Add(prop);
                }
                    
                Travel = await _travelRepository.GetTravelByIdAsync<GuideMainBindingModel>(_travelId);
            }
        }

        #endregion

    }

    
    
}
