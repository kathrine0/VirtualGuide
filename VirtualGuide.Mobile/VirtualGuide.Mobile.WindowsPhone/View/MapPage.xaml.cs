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
using System.Collections.ObjectModel;
using VirtualGuide.Mobile.ViewModel.GuideList;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.BindingModel;
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
        private MapViewModel _viewModel = new MapViewModel(typeof(PlaceMain));
        
        public MapPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel.ZoomingMapToPoint += ViewModel_ZoomingMapToPoint;

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
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

        public MapViewModel ViewModel
        {
            get { return this._viewModel; }
            set { _viewModel = value; }
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
            ViewModel.LoadData(travelId);

            if (e.PageState != null && e.PageState.ContainsKey("Latitude")
                && e.PageState.ContainsKey("Longitude") && e.PageState.ContainsKey("Zoom"))
            {
                ViewModel.ZoomLevel = (double)e.PageState["Zoom"];
                ViewModel.Center = new Geopoint(new BasicGeoposition() { Latitude = (double)e.PageState["Latitude"], Longitude = (double)e.PageState["Longitude"] });
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
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["Latitude"] = ViewModel.Center.Position.Latitude;
            e.PageState["Longitude"] = ViewModel.Center.Position.Longitude;
            e.PageState["Zoom"] = ViewModel.ZoomLevel;
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

        #region Positioning

        

        

        #endregion

        #region Compass

        

        #endregion

        #region Maps

        #endregion

        #region MyPosition Button
        
        #endregion


        private void Filter_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

    }

}
