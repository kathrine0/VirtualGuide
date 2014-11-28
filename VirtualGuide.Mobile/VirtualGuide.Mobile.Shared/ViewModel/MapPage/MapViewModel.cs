using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.Repository;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using System.Collections.Specialized;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.ViewManagement;
using Windows.Graphics.Display;

namespace VirtualGuide.Mobile.ViewModel.MapPage
{
    public class MapViewModel : BaseViewModel
    {

        #region events

        public event Action<Geopoint, double> ZoomingMapToPoint;

        #endregion

        #region constructors

        public MapViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this._uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());


            _travel = new MapTravelBindingModel();
            _places = new ObservableCollection<MapPlaceBindingModel>();
            _categories = new ItemsChangeObservableCollection<CategoryVisibilityModel>();

            BindCommands();
            
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

        public RelayCommand<MapControl> InitializeMapCommand { get; set; }
        public RelayCommand HideDetailCloudsCommand { get; set; }
        public RelayCommand LocateMeCommand { get; set; }
        public RelayCommand ShowHideFilterScreenCommand { get; set; }

        #endregion

        #region public properties
        private Geopoint _userGeoposition;
        public Geopoint UserGeoposition
        {
            get { return _userGeoposition; }
            set { Set(ref _userGeoposition, value); }
        }

        private double _zoomLevel;
        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { Set(ref _zoomLevel, value); }
        }

        private Geopoint _center;
        public Geopoint Center
        {
            get { return _center; }
            set { Set(ref _center, value); }
        }

        private bool _compassIsActive;
        public bool CompassIsActive
        {
            get { return _compassIsActive; }
            set { Set(ref _compassIsActive, value); }
        }

        private bool _isMarkerVisible;
        public bool IsMarkerVisible
        {
            get { return _isMarkerVisible; }
            set { Set(ref _isMarkerVisible, value); }
        }

        private double _heading;
        public double Heading
        {
            get { return _heading; }
            set { Set(ref _heading, value); }
        }

        private MapTravelBindingModel _travel;
        public MapTravelBindingModel Travel
        {
            get { return _travel; }
            private set { Set(ref _travel, value); }
        }

        private ObservableCollection<MapPlaceBindingModel> _places;
        public ObservableCollection<MapPlaceBindingModel> Places
        {
            get { return _places; }
            set { Set(ref _places, value); }
        }

        private LocationEllipseParams _locationEllipse;
        public LocationEllipseParams LocationEllipse
        {
            get { return _locationEllipse; }
            private set { Set(ref _locationEllipse, value); }
        }

        private CompassPathParams _compassPath;
        public CompassPathParams CompassPath
        {
            get { return _compassPath; }
            private set { Set(ref _compassPath, value); }
        }

        private bool _calibrationInProgress;
        public bool CalibrationInProgress
        {
            get { return _calibrationInProgress; }
            private set { Set(ref _calibrationInProgress, value); }
        }

        private ItemsChangeObservableCollection<CategoryVisibilityModel> _categories;
        public ItemsChangeObservableCollection<CategoryVisibilityModel> Categories
        {
            get { return _categories; }
            set
            {
                Set(ref _categories, value);
            }
        }

        private bool _filterMode = false;
        public bool FilterMode
        {
            get { return _filterMode; }
            private set
            {
                Set(ref _filterMode, value);
            }
        }

        private bool _mapInitialized = false;
        public bool MapInitialized
        {
            get { return _mapInitialized; }
            set
            {
                Set(ref _mapInitialized, value);
            }
        }
        

        #endregion

        #region private properties

        private int _travelId;
        private TravelRepository _travelRepository = new TravelRepository();
        private PlaceRepository _placeRepository = new PlaceRepository();

        private Compass _compass;
        private Geolocator _geolocator = null;

        private int? _visibleDetailsPlaceId;
        private bool _markerTapped = false;

        private TaskFactory _uiFactory;

        /// <summary>
        /// Is map currently centered to User Position
        /// </summary>
        private bool _centeredToPosition = false;


        #endregion

        #region private methods
        private void BindCommands()
        {
            InitializeMapCommand = new RelayCommand<MapControl>(InitializeMapExecute);
            HideDetailCloudsCommand = new RelayCommand(HideDetailCloudsExecute);
            LocateMeCommand = new RelayCommand(LocateMeExecute);
            ShowHideFilterScreenCommand = new RelayCommand(ShowHideFilterScreenExecute);
        }

        private async void SetUpUI()
        {
            //Hide system tray
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            //Enable orientations
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        private async void RestoreUI()
        {
            //Restore system tray
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.ShowAsync();

            //Disable orientations
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

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
        public async void LoadData()
        {
            IsWorkInProgress = true;
            Categories.Clear();
            Places.Clear();

            SetupGPSAndCompass();

            if (_travelId != 0)
            {
                var travelTask = _travelRepository.GetTravelByIdAsync<MapTravelBindingModel>(_travelId);

                var places = await _placeRepository.GetParentPlacesByTravelIdAsync<MapPlaceBindingModel>(_travelId);
                Categories = _placeRepository.GetCategoryVisibilityCollection(places);


                foreach (var place in places)
                {
                    place.Category = Categories.Where(x => x.Name == place.CategoryName).FirstOrDefault();
                    place.ShowDetailCloudCommand = new RelayCommand<MapPlaceBindingModel>(ShowDetailCloudExecute);
                    place.NavigateToPlaceDetailsCommand = new RelayCommand<MapPlaceBindingModel>(NavigateToPlaceDetailsExecute);

                    Places.Add(place);
                }

                Travel = await travelTask;
            }


            IsWorkInProgress = false;
        }

        public void InitializeMapExecute(MapControl mapControl)
        {
            if (MapInitialized || 
                mapControl == null || 
                mapControl.LoadingStatus != MapLoadingStatus.Loaded ||
                Travel == null ||
                Travel.Id != _travelId)
                return;

            //set initial parameters

            var zoomLevel = Travel.ZoomLevel;
            var center = new Geopoint(new BasicGeoposition() { Latitude = Travel.Latitude, Longitude = Travel.Longitude });

            if (ZoomingMapToPoint != null)
            {
                ZoomingMapToPoint(center, zoomLevel);
            }            
            
            //ZoomLevel = Travel.ZoomLevel;
            //Center = new Geopoint(new BasicGeoposition() { Latitude = Travel.Latitude, Longitude = Travel.Longitude });

            //setup geolocation
            MapInitialized = true;
        }

        public void HideDetailCloudsExecute()
        {
            if (_markerTapped) return;
            HideAllClouds();

        }

        public void NavigateToPlaceDetailsExecute(MapPlaceBindingModel place)
        {
            _navigationService.NavigateTo("PlaceMain", place.Id);
        }

        public void ShowDetailCloudExecute(MapPlaceBindingModel place)
        {

            HideAllClouds();

            place.DetailsVisibility = true;

            Places.Move(Places.IndexOf(place), Places.Count - 1);

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

        public void ShowHideFilterScreenExecute()
        {
            FilterMode = !FilterMode;
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
            await _uiFactory.StartNew(() =>
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
            await _uiFactory.StartNew(() =>
            {
                Geoposition pos = e.Position;

                UserGeoposition = new Geopoint(pos.Coordinate.Point.Position);

                if (_centeredToPosition && ZoomingMapToPoint != null)
                {
                    ZoomingMapToPoint(UserGeoposition, ZoomLevel);
                }
            });
        }

        #endregion

        #region Compass Events

        async private void ReadingChanged(object sender, CompassReadingChangedEventArgs e)
        {
            await _uiFactory.StartNew(() =>
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

                if (ZoomingMapToPoint != null)
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

        #region navigation

        public override void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _travelId = (int)e.NavigationParameter;
            LoadData();

            SetUpUI();


            if (e.PageState != null &&
                e.PageState.ContainsKey(String.Format("Travel{0}{1}", _travelId, "Latitude")) &&
                e.PageState.ContainsKey(String.Format("Travel{0}{1}", _travelId, "Longitude")) &&
                e.PageState.ContainsKey(String.Format("Travel{0}{1}", _travelId, "Zoom")))
            {
                ZoomLevel = (double)e.PageState[String.Format("Travel{0}{1}", _travelId, "Zoom")];
                Center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = (double)e.PageState[String.Format("Travel{0}{1}", _travelId, "Latitude")],
                    Longitude = (double)e.PageState[String.Format("Travel{0}{1}", _travelId, "Longitude")]
                });

                MapInitialized = true;
            }
            else
            {
                MapInitialized = false;
            }

            base.NavigationHelper_LoadState(sender, e);
        }

        public override void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            RestoreUI();

            if (Center != null)
            {
                e.PageState[String.Format("Travel{0}{1}", _travelId, "Latitude")] = Center.Position.Latitude;
                e.PageState[String.Format("Travel{0}{1}", _travelId, "Longitude")] = Center.Position.Longitude;
                e.PageState[String.Format("Travel{0}{1}", _travelId, "Zoom")] = ZoomLevel;
            }

            base.NavigationHelper_SaveState(sender, e);
        }

        #endregion
    }
}