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
        
        public MapView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

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

            if (_compass != null)
            {
                _compass.ReadingChanged -= new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
                // Restore the default report interval to release resources while the sensor is not in use
                _compass.ReportInterval = 0;
            }

            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Positioning

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Geoposition pos = e.Position;

                MapElements.UserGeoposition = new Geopoint(pos.Coordinate.Point.Position);
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
                        MapElements.MarkerVisibility = Windows.UI.Xaml.Visibility.Visible;
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
        }

        private void LocationEllipseInactive()
        {
            LocationEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 83, 83, 83));
            LocationEllipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 195, 195, 195));
        }

        #endregion

        #region Compass

        async private void ReadingChanged(object sender, CompassReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CompassReading reading = e.Reading;
                
                if (reading.HeadingTrueNorth != null)
                {
                    _mapElement.Heading = reading.HeadingTrueNorth.Value;
                }
                else if (reading.HeadingMagneticNorth != null)
                {
                    _mapElement.Heading = reading.HeadingMagneticNorth;
                }
                else
                {
                    //calibrate
                    //ScenarioOutput_TrueNorth.Text = "No data";
                }
                switch (reading.HeadingAccuracy)
                {
                    case MagnetometerAccuracy.Unknown:
                        //ScenarioOutput_HeadingAccuracy.Text = "Unknown";
                        break;
                    case MagnetometerAccuracy.Unreliable:
                        //ScenarioOutput_HeadingAccuracy.Text = "Unreliable";
                        break;
                    case MagnetometerAccuracy.Approximate:
                        //ScenarioOutput_HeadingAccuracy.Text = "Approximate";
                        break;
                    case MagnetometerAccuracy.High:
                        //ScenarioOutput_HeadingAccuracy.Text = "High";
                        break;
                    default:
                        //ScenarioOutput_HeadingAccuracy.Text = "No data";
                        break;
                }
            });
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

        private void MenuPlaces_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GuidePlaces), _travel.Id);
        }

        private void SetupGPSAndCompass()
        {
            App.Geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            App.Geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            _compass = Compass.GetDefault();
            if (_compass != null)
            {
                // Select a report interval that is both suitable for the purposes of the app and supported by the sensor.
                // This value will be used later to activate the sensor.
                uint minReportInterval = _compass.MinimumReportInterval;
                _compass.ReportInterval = minReportInterval > 16 ? minReportInterval : 16; ;

                _compass.ReadingChanged += new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
            }
            else
            {
                //TODO
                //rootPage.NotifyUser("No compass found", NotifyType.ErrorMessage);
            }
        }

        #region MyPosition Button
        private async void LocateMeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (MapElements.UserGeoposition != null)
            {
                await Maps.TrySetViewAsync(MapElements.UserGeoposition, 19, null, null, MapAnimationKind.Default);

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
