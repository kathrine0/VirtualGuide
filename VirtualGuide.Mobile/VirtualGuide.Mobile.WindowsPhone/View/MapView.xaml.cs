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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MapView : Page
    {
        private TravelViewModel _travel = null;
        private List<MapPlaceViewModel> _places = null;
        private TravelRepository _travelRepository = new TravelRepository();
        private PlaceRepository _placeRepository = new PlaceRepository();

        private int? _visibleDetailsPlaceId;
        private bool _markerTapped = false;
        
        private NavigationHelper navigationHelper;
        
        private MapElements _mapElements = new MapElements();
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        

        private Geolocator _geolocator = null;

        public MapView()
        {
            this.InitializeComponent();

            _geolocator = new Geolocator();
            _geolocator.ReportInterval = 1000;
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }


        public MapElements MapElements
        {
            get { return this._mapElements; }
            set { _mapElements = value; }
        }

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
            _places = await _placeRepository.GetSimplePlaces(travelId);

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
        }

        #region NavigationHelper registration

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
            _geolocator.PositionChanged -= new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            _geolocator.StatusChanged -= new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Positioning

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Geoposition pos = e.Position;

                MapElements.UserGeoposition = new Geopoint(new BasicGeoposition() { Latitude = pos.Coordinate.Latitude, Longitude = pos.Coordinate.Longitude });
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
                        MessageBoxHelper.Show("Location feature is turned off");
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

        private void MarkerImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

            HideDetailClouds();

            var element = (MapPlaceViewModel)((Image)sender).DataContext;
            element.DetailsVisibility = true;

            _visibleDetailsPlaceId = element.Id;
            _markerTapped = true;
            WaitMarkerTap();
            
        }

        private async void WaitMarkerTap()
        {
            await Task.Delay(100);
            _markerTapped = false;
        }

        private void HideDetailClouds()
        {
            if (_visibleDetailsPlaceId != null)
            {
                _places.Find(place => place.Id == _visibleDetailsPlaceId).DetailsVisibility = false;
            }

            _visibleDetailsPlaceId = null;
        }

        private void Maps_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            if (_markerTapped) return;
            HideDetailClouds(); 
        }


        private async void LocateMeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (MapElements.UserGeoposition != null)
            {
                await Maps.TrySetViewAsync(MapElements.UserGeoposition, 19, null, null, MapAnimationKind.Default);

            }
        }

        private async void Maps_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Wait if travel has not been loaded yet
            if (_travel == null)
            {
                await Task.Delay(300);
            }
            
            //set initial parameters
            var zoomLevel = _travel.ZoomLevel;
            var center = new Geopoint(new BasicGeoposition() { Latitude = _travel.Latitude, Longitude = _travel.Longitude });

            //zoom map
            await Maps.TrySetViewAsync(center, zoomLevel, null, null, MapAnimationKind.Default);

            //wait if places has not been loaded
            if (_places == null)
            {
                await Task.Delay(300);
            }

            //load places
            MapElements.Places = _places;

            //setup geolocation
            _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            _geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);
        }

        
    }

    [ImplementPropertyChanged]
    public class MapElements
    {
        public Geopoint UserGeoposition { get; set; }
        public double ZoomLevel { get; set; }
        public Geopoint Center { get; set; }

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
}
