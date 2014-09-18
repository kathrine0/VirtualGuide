using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.Repository;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.ViewModel.MapPage
{
    [ImplementPropertyChanged]
    public class MapViewModel
    {
        #region readonly properties

        private readonly Type _placeMain;

        #endregion

        #region events

        public event Action<Geopoint, double> ZoomingMapToPoint;

        #endregion

        #region constructors

        public MapViewModel(Travel travel, Type placeMain) : this(placeMain)
        {
            ZoomLevel = travel.ZoomLevel;
            Center = new Geopoint(new BasicGeoposition() { Latitude = travel.Latitude, Longitude = travel.Longitude });

        }

        public MapViewModel(Type placeMain)
        {
            _placeMain = placeMain;

            uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

            Initialize();

            Travel = new MapTravelBindingModel();
            Places = new ObservableCollection<MapPlaceBindingModel>();
            Categories = new ObservableCollection<Tuple<bool, string>>();
            LocationEllipse = new LocationEllipseParams()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 83, 83, 83)),
                Stroke = new SolidColorBrush(Color.FromArgb(255, 195, 195, 195)),
                Width = 20,
                Height = 20,
            };

            CompassPath = new CompassPathParams()
            {
                CompassMode = false,
                Fill = new SolidColorBrush(Colors.Gray)
            };
        }

        #endregion

        #region commands

        public DelegateCommand InitializeMapCommand { get; set; }
        public DelegateCommand HideDetailCloudsCommand { get; set; }
        public DelegateCommand LocateMeCommand { get; set; }
        public DelegateCommand ShowFilterScreenCommand { get; set; }

        #endregion

        #region public properties

        public Geopoint UserGeoposition { get; set; }
    
        public double ZoomLevel { get; set; }
      
        public Geopoint Center { get; set; }
       
        public bool CompassIsActive { get; set; }

        public bool IsMarkerVisible { get; set; }

        public double Heading { get; set; }

        public MapTravelBindingModel Travel
        {
            get;
            private set;
        }

        public ObservableCollection<MapPlaceBindingModel> Places
        {
            get;
            private set;
        }

        public LocationEllipseParams LocationEllipse { get; set; }

        public CompassPathParams CompassPath { get; set; }

        public bool CalibrationInProgress { get; set; }

        //public List<MapPlaceViewModel> Data
        //{
        //    get;
        //    set;
        //}

        //public List<MapPlaceViewModel> FilteredData
        //{
        //    get
        //    {
        //        if (Categories == null) return null;

        //        var visibleCategories = Categories.Where(x => x.Item1 == true).Select(x => x.Item2);

        //        return Data.Where(x => visibleCategories.Contains(x.Category)).ToList();
        //    }
        //}

        public ObservableCollection<Tuple<bool, string>> Categories { get;set; }

        public bool FilterMode { get; set; }

        #endregion

        #region private properties

        private int _travelId;
        private TravelRepository _travelRepository = new TravelRepository();
        private PlaceRepository _placeRepository = new PlaceRepository();

        private Compass _compass;
        private Geolocator _geolocator = null;

        private int? _visibleDetailsPlaceId;
        private bool _markerTapped = false;

        private TaskFactory uiFactory;

        /// <summary>
        /// Is map currently centered to User Position
        /// </summary>
        private bool _centeredToPosition = false;
        

        #endregion

        #region private methods
        private void Initialize()
        {
            InitializeMapCommand = new DelegateCommand(InitializeMapExecute);
            HideDetailCloudsCommand = new DelegateCommand(HideDetailCloudsExecute);
            LocateMeCommand = new DelegateCommand(LocateMeExecute);
            ShowFilterScreenCommand = new DelegateCommand(ShowFilterScreenExecute);
        }

        private void HideAllClouds()
        {
            if (_visibleDetailsPlaceId != null)
            {
                Places.Where(place => place.Id == _visibleDetailsPlaceId).All(x => x.DetailsVisibility = false);
            }

            _visibleDetailsPlaceId = null;
        }

        private async void WaitMarkerTap()
        {
            await Task.Delay(100);
            _markerTapped = false;
        }

        private void LocationEllipseActive()
        {
            LocationEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 63, 162, 63));
            LocationEllipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            LocationEllipse.Width = 15;
            LocationEllipse.Height = 15;

            CompassPath.CompassMode = true;
        }

        private void LocationEllipseInactive()
        {
            LocationEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 83, 83, 83));
            LocationEllipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 195, 195, 195));
            LocationEllipse.Width = 20;
            LocationEllipse.Height = 20;

            CompassPath.CompassMode = false;
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
                Travel = await _travelRepository.GetTravelByIdAsync<MapTravelBindingModel>(_travelId);

                var places = await _placeRepository.GetParentPlacesByTravelIdAsync<MapPlaceBindingModel>(_travelId);

                foreach (var place in places)
                {
                    place.ShowDetailCloudCommand = new DelegateCommand<MapPlaceBindingModel>(ShowDetailCloudExecute);
                    place.NavigateToPlaceDetailsCommand = new DelegateCommand<MapPlaceBindingModel>(NavigateToPlaceDetailsExecute);

                    Places.Add(place);
                }

                var categories = Places.Select(x => x.Category).Distinct();

                Categories = new ObservableCollection<Tuple<bool, string>>(categories.Select(x => new Tuple<bool, string>(true, x)));
            }
        }

        public async void InitializeMapExecute()
        {
            //Wait if travel has not been loaded yet
            if (Travel == null)
            {
                await Task.Delay(300);
            }

            //set initial parameters
            if (ZoomLevel == 0 || Center == null)
            {
                var zoomLevel = Travel.ZoomLevel;
                var center = new Geopoint(new BasicGeoposition() { Latitude = Travel.Latitude, Longitude = Travel.Longitude });

                if (ZoomingMapToPoint != null)
                {
                    ZoomingMapToPoint(center, zoomLevel);
                }

            }

            //setup geolocation
            SetupGPSAndCompass();
        }

        public void HideDetailCloudsExecute()
        {
            if (_markerTapped) return;
            HideAllClouds();
            
        }

        public void NavigateToPlaceDetailsExecute(MapPlaceBindingModel place)
        {
            App.RootFrame.Navigate(_placeMain, place.Id);
        }

        public void ShowDetailCloudExecute(MapPlaceBindingModel place)
        {

            HideAllClouds();

            place.DetailsVisibility = true;

            _visibleDetailsPlaceId = place.Id;
            _markerTapped = true;
            WaitMarkerTap();

        }

        public void LocateMeExecute()
        {
            //if already centered
            if (_centeredToPosition)
            {
                if (!CompassIsActive)
                {
                    ActivateCompass();
                }
                else
                {
                    DeactivateCompass();
                    _centeredToPosition = false;
                }
            }
            else if (UserGeoposition != null)
            {
                if (ZoomingMapToPoint != null)
                {
                    ZoomingMapToPoint(UserGeoposition, 15);
                }
                _centeredToPosition = true;
            }
        }

        public void ShowFilterScreenExecute()
        {
            FilterMode = true;
        }

        #endregion

        #region GPS and Compass
        private void SetupGPSAndCompass()
        {
            _geolocator = new Geolocator();
            _geolocator.MovementThreshold = 5;
            _geolocator.DesiredAccuracy = PositionAccuracy.High;

            _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            _geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            _compass = Compass.GetDefault();

        }

        #region GPS Events

        private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            await uiFactory.StartNew( () =>
            {
                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        LocationEllipseActive();
                        IsMarkerVisible = true;
                        break;

                    case PositionStatus.Disabled:
                        LocationEllipseInactive();
                        MessageBoxHelper.ShowNoLocation();
                        break;
                    case PositionStatus.Initializing:
                    case PositionStatus.NoData:
                    case PositionStatus.NotInitialized:
                    case PositionStatus.NotAvailable:
                    default:
                        LocationEllipseInactive();
                        break;

                }
            });
        }

        private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await uiFactory.StartNew(() =>
            {
                Geoposition pos = e.Position;

                UserGeoposition = new Geopoint(pos.Coordinate.Point.Position);

                if (_centeredToPosition && ZoomingMapToPoint != null)
                {
                    //Task.Run(() => Maps.TrySetViewAsync(_mapElement.UserGeoposition));
                    //Will it work?
                    ZoomingMapToPoint(UserGeoposition, ZoomLevel);

                }
            });
        }

        #endregion

        #region Compass Events

        async private void ReadingChanged(object sender, CompassReadingChangedEventArgs e)
        {
            await uiFactory.StartNew(() =>
            {
                CompassReading reading = e.Reading;

                if (reading == null) return;

                if (reading.HeadingTrueNorth != null)
                {
                    Heading = reading.HeadingTrueNorth.Value;
                }
                else
                {
                    Heading = reading.HeadingMagneticNorth;
                }

                switch (reading.HeadingAccuracy)
                {
                    //case MagnetometerAccuracy.Unknown:
                    //    //ScenarioOutput_HeadingAccuracy.Text = "Unknown";
                    //    break;
                    case MagnetometerAccuracy.Unreliable:
                        if (!CalibrationInProgress)
                        {
                            CalibrationInProgress = true;
                        }
                        break;
                    case MagnetometerAccuracy.Approximate:
                    case MagnetometerAccuracy.High:
                    default:
                        if (CalibrationInProgress)
                        {
                            CalibrationInProgress = false;
                        }
                        break;
                }
            });
        }

        private void ActivateCompass()
        {
            if (_compass != null && !CompassIsActive)
            {
                uint minReportInterval = _compass.MinimumReportInterval;
                _compass.ReportInterval = minReportInterval > 16 ? minReportInterval : 16; ;

                _compass.ReadingChanged += new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
                CompassPath.Fill = new SolidColorBrush(Colors.White);
                CompassIsActive = true;
            }
        }

        private void DeactivateCompass()
        {
            if (_compass != null && CompassIsActive)
            {
                _compass.ReadingChanged -= new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
                // Restore the default report interval to release resources while the sensor is not in use
                _compass.ReportInterval = 0;
                CompassPath.Fill = new SolidColorBrush(Colors.Gray);
                CompassIsActive = false;

                if(ZoomingMapToPoint != null)
                {
                    ZoomingMapToPoint(Center, ZoomLevel);
                }
            }
        }

        #endregion

        #endregion

        #region helper class

        [ImplementPropertyChanged]
        public class LocationEllipseParams
        {
            public Brush Fill { get; set; }
            public Brush Stroke { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            
        }

        [ImplementPropertyChanged]
        public class CompassPathParams
        {
            public bool CompassMode { get; set; }
            public Brush Fill { get; set; }
        }

        #endregion
    }
}
