using VirtualGuide.Mobile.Common;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using VirtualGuide.Mobile.ViewModel;
using VirtualGuide.Mobile.Repository;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Windows.UI;
using PropertyChanged;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Maps;
using System.Threading.Tasks;
using System.Diagnostics;
using VirtualGuide.Mobile.Helper;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MapView : Page
    {
        private TravelViewModel _travel = null;
        private TravelRepository _travelRepository = new TravelRepository();
        private PlaceRepository _placeRepository = new PlaceRepository();

        private int? _visibleDetailsPlaceId;
        private bool _markerTapped = false;
        
        private NavigationHelper navigationHelper;
        
        private MapElement _mapElement = new MapElement();

        private Compass _compass;

        /// <summary>
        /// Is map currently centered to User Position
        /// </summary>
        private bool _centeredToPosition = false;
        private bool _calibrationInProgress = false;
        
        public MapView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public MapElement MapElements
        {
            get { return this._mapElement; }
            set { _mapElement = value; }
        }

        #endregion


        #region NavigationHelper

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //Hide system tray
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            var travelId = (int)e.NavigationParameter;
            _travel = await _travelRepository.GetTravelByIdAsync(travelId);
            _mapElement.Places = await _placeRepository.GetPlacesForMap(travelId);

            if (e.PageState != null && e.PageState.ContainsKey("Latitude")
                && e.PageState.ContainsKey("Longitude") && e.PageState.ContainsKey("Zoom"))
            {
                _mapElement.ZoomLevel = (double)e.PageState["Zoom"];
                _mapElement.Center = new Geopoint(new BasicGeoposition() { Latitude = (double)e.PageState["Latitude"], Longitude = (double)e.PageState["Longitude"] });
            }

            _mapElement.Heading = 0;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["Latitude"] = Maps.Center.Position.Latitude;
            e.PageState["Longitude"] = Maps.Center.Position.Longitude;
            e.PageState["Zoom"] = Maps.ZoomLevel;
        }

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Geolocator.PositionChanged -= new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            App.Geolocator.StatusChanged -= new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            

            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (_calibrationInProgress)
            {
                e.Handled = true;

                DeactivateCompass();
                CalibrationScreen.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                _calibrationInProgress = false;

            }
        }

        #endregion

        #region Positioning

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Geoposition pos = e.Position;

                _mapElement.UserGeoposition = new Geopoint(pos.Coordinate.Point.Position);
            });
        }

        /// <summary>
        /// This is the event handler for StatusChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        LocationEllipseActive();
                        _mapElement.MarkerVisibility = Windows.UI.Xaml.Visibility.Visible;
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

        private void LocationEllipseActive()
        {
            LocationEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 63, 162, 63));
            LocationEllipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            LocationEllipse.Width = 15;
            LocationEllipse.Height = 15;

            CompassPath.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void LocationEllipseInactive()
        {
            LocationEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 83, 83, 83));
            LocationEllipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 195, 195, 195));
            LocationEllipse.Width = 20;
            LocationEllipse.Height = 20;

            CompassPath.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        #endregion

        #region Compass

        async private void ReadingChanged(object sender, CompassReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CompassReading reading = e.Reading;

                if (reading == null) return;

                if (reading.HeadingTrueNorth != null)
                {
                    _mapElement.Heading = reading.HeadingTrueNorth.Value;
                }
                else
                {
                    _mapElement.Heading = reading.HeadingMagneticNorth;
                }

                switch (reading.HeadingAccuracy)
                {
                    //case MagnetometerAccuracy.Unknown:
                    //    //ScenarioOutput_HeadingAccuracy.Text = "Unknown";
                    //    break;
                    case MagnetometerAccuracy.Unreliable:
                        if (!_calibrationInProgress)
                        {
                            CalibrationScreen.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            _calibrationInProgress = true;
                        }
                        break;
                    case MagnetometerAccuracy.Approximate:
                    case MagnetometerAccuracy.High:
                    default:
                        if (_calibrationInProgress)
                        {
                            CalibrationScreen.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                            _calibrationInProgress = false;
                        }
                        break;
                }
            });
        }

        private void ActivateCompass()
        {
            if (_compass != null && !_mapElement.CompassIsActive)
            {
                uint minReportInterval = _compass.MinimumReportInterval;
                _compass.ReportInterval = minReportInterval > 16 ? minReportInterval : 16; ;

                _compass.ReadingChanged += new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
                CompassPath.Fill = new SolidColorBrush(Colors.White);
                _mapElement.CompassIsActive = true;
            }
        }

        private async void DeactivateCompass()
        {
            if (_compass != null && _mapElement.CompassIsActive)
            {
                _compass.ReadingChanged -= new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
                // Restore the default report interval to release resources while the sensor is not in use
                _compass.ReportInterval = 0;
                CompassPath.Fill = new SolidColorBrush(Colors.Gray);
                _mapElement.CompassIsActive = false;

                await Maps.TrySetViewAsync(_mapElement.Center, _mapElement.ZoomLevel, 0, null, MapAnimationKind.Default);
            }
        }

        #endregion

        private void Maps_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            if (_markerTapped) return;
            HideDetailClouds(); 
        }

        private async void Maps_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Wait if travel has not been loaded yet
            if (_travel == null)
            {
                await Task.Delay(300);
            }

            var animation = MapAnimationKind.None;

            //set initial parameters
            if (_mapElement.ZoomLevel == 0 || _mapElement.Center == null)
            {
                var zoomLevel = _travel.ZoomLevel;
                var center = new Geopoint(new BasicGeoposition() { Latitude = _travel.Latitude, Longitude = _travel.Longitude });
                animation = MapAnimationKind.Default;
              
                //zoom map
                await Maps.TrySetViewAsync(center, zoomLevel, null, null, animation);
            }

            //setup geolocation
            SetupGPSAndCompass();
        }

        private void Maps_CenterChanged(MapControl sender, object args)
        {
            DeactivateCompass();
            _centeredToPosition = false;
        }

        private void MenuPlaces_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GuidePlaces), _travel.Id);
        }

        private void SetupGPSAndCompass()
        {
            App.Geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            App.Geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            _compass = Compass.GetDefault();

        }

        #region MyPosition Button
        private async void LocateMeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //if already centered
            if (_centeredToPosition)
            {
                if (!_mapElement.CompassIsActive)
                {
                    ActivateCompass();
                }
                else
                {
                    DeactivateCompass();
                }
            }
            else if (_mapElement.UserGeoposition != null)
            {
                await Maps.TrySetViewAsync(_mapElement.UserGeoposition, 15, null, null, MapAnimationKind.Default);
                _centeredToPosition = true;
            }
        }
        
        #endregion

        #region Map Markers
        private void MapMarkerGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var clickedItem = (MapPlaceViewModel)((Grid)sender).DataContext;

            Frame.Navigate(typeof(PlaceMain), clickedItem.Id);
        }
        private async void WaitMarkerTap()
        {
            await Task.Delay(100);
            _markerTapped = false;
        }
        private void MarkerImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

            HideDetailClouds();

            var element = (MapPlaceViewModel)((Image)sender).DataContext;
            element.DetailsVisibility = true;

            _visibleDetailsPlaceId = element.Id;
            _markerTapped = true;
            WaitMarkerTap();
            
        }
        private void HideDetailClouds()
        {
            if (_visibleDetailsPlaceId != null)
            {
                _mapElement.Places.Find(place => place.Id == _visibleDetailsPlaceId).DetailsVisibility = false;
            }

            _visibleDetailsPlaceId = null;
        }
        
        #endregion

        
    }

    #region MapElement Class

    [ImplementPropertyChanged]
    public class MapElement
    {
        public Geopoint UserGeoposition { get; set; }
        public double ZoomLevel { get; set; }
        public Geopoint Center { get; set; }

        public bool CompassIsActive { get; set; }
        public double Heading { get; set; }

        private List<MapPlaceViewModel> _places = new List<MapPlaceViewModel>();
        public List<MapPlaceViewModel> Places { 
            get
            {
                return _places;
            }
            set
            {
                _places = new List<MapPlaceViewModel>(value);
            }
        }

        private Windows.UI.Xaml.Visibility _markerVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        public Windows.UI.Xaml.Visibility MarkerVisibility
        {
            get { return _markerVisibility; }
            set { _markerVisibility = value; }
        }
        
    }

    #endregion
}
