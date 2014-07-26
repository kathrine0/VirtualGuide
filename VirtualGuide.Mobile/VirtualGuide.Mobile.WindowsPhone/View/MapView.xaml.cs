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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MapView : Page
    {
        private TravelViewModel _travel;
        private List<MapPlaceViewModel> _places = new List<MapPlaceViewModel>();
        private TravelRepository _travelRepository = new TravelRepository();
        private PlaceRepository _placeRepository = new PlaceRepository();

        private int? _visibleDetailsPlaceId;
        
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

            _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            _geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);


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
            statusBar.HideAsync();

            var travelId = (int)e.NavigationParameter;
            _travel = await _travelRepository.GetTravelByIdAsync(travelId);
            _places = await _placeRepository.GetSimplePlaces(travelId);

            _mapElements.ZoomLevel = _travel.ZoomLevel;
            _mapElements.Center = new Geopoint(new BasicGeoposition() { Latitude = _travel.Latitude, Longitude = _travel.Longitude });
            _mapElements.Places = _places;

            //this.DefaultViewModel["ZoomLevel"] = _mapElements.ZoomLevel;
            //this.DefaultViewModel["Center"] = _mapElements.Center;
            //defaultViewModel["Maps"] = _mapElements;
            //this.DefaultViewModel["MapElements"] = _places;
            //this.DefaultViewModel["UserPosition"] = _mapElements.UserGeoposition;
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

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {

            HideDetailClouds();

            var element = (MapPlaceViewModel)((Image)sender).DataContext;
            element.DetailsVisibility = true;

            _visibleDetailsPlaceId = element.Id;
        }

        private void HideDetailClouds()
        {
            if (_visibleDetailsPlaceId != null)
            {
                _places.Find(place => place.Id == _visibleDetailsPlaceId).DetailsVisibility = false;
            }

            _visibleDetailsPlaceId = null;
        }

        private void Maps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //HideDetailClouds(); 
        }


        private void LocateMeGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (MapElements.UserGeoposition != null)
            {
                MapElements.ZoomLevel = 19;
                MapElements.Center = MapElements.UserGeoposition;
                //Maps.ZoomLevel = 15;
            }
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
