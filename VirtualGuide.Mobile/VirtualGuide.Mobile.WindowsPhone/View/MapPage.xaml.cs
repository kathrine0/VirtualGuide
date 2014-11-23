using VirtualGuide.Mobile.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls.Maps;
using VirtualGuide.Mobile.Helper;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using VirtualGuide.Mobile.ViewModel.MapPage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MapPage : Page
    {
        private NavigationHelper navigationHelper;
        private MapViewModel viewModel;
        private int TravelId;

        public MapPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            viewModel = DataContext as MapViewModel;

            viewModel.ZoomingMapToPoint += ViewModel_ZoomingMapToPoint;

            
        }

        private async void ViewModel_ZoomingMapToPoint(Geopoint center, double zoomLevel)
        {
            await Maps.TrySetViewAsync(center, zoomLevel, null, null, MapAnimationKind.Default);
        }

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
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
            TravelId = (int)e.NavigationParameter;
            viewModel.LoadData(TravelId);

            //Hide system tray
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            //Enable orientations
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;

            await statusBar.HideAsync();

            if (e.PageState != null && 
                e.PageState.ContainsKey(String.Format("Travel{0}{1}", TravelId, "Latitude")) && 
                e.PageState.ContainsKey(String.Format("Travel{0}{1}", TravelId, "Longitude")) && 
                e.PageState.ContainsKey(String.Format("Travel{0}{1}", TravelId, "Zoom")))
            {
                viewModel.ZoomLevel = (double)e.PageState[String.Format("Travel{0}{1}", TravelId, "Zoom")];
                viewModel.Center = new Geopoint(new BasicGeoposition() {
                    Latitude  = (double) e.PageState[String.Format("Travel{0}{1}", TravelId, "Latitude")],
                    Longitude = (double) e.PageState[String.Format("Travel{0}{1}", TravelId, "Longitude")] 
                });
            
                viewModel.MapInitialized = true;
            }
            else
            {
                viewModel.MapInitialized = false;
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private async void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            //Restore system tray
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            //Disable orientations
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            await statusBar.ShowAsync();

            e.PageState[String.Format("Travel{0}{1}", TravelId, "Latitude")] = viewModel.Center.Position.Latitude;
            e.PageState[String.Format("Travel{0}{1}", TravelId, "Longitude")] = viewModel.Center.Position.Longitude;
            e.PageState[String.Format("Travel{0}{1}", TravelId, "Zoom")] = viewModel.ZoomLevel;
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
            //_geolocator.PositionChanged -= new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            //_geolocator.StatusChanged -= new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //if (CalibrationInProgress)
            //{
            //    e.Handled = true;

            //    DeactivateCompass();
            //    CalibrationScreen.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //    CalibrationInProgress = false;

            //}
        }

        #endregion
    }

}
