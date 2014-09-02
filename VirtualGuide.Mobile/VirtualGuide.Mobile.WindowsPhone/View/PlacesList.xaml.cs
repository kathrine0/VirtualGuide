using VirtualGuide.Mobile.Common;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using VirtualGuide.Mobile.ViewModel;
using VirtualGuide.Mobile.Repository;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Helper;
using Windows.UI.Core;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuidePlaces : Page
    {
        private TravelViewModel _travel;
        private TravelRepository _travelRepository = new TravelRepository();
        private PlaceRepository _placeRepository = new PlaceRepository();

        private PlaceViewModel _placeViewModel = new PlaceViewModel();

        private NavigationHelper navigationHelper;

        public GuidePlaces()
        {
            this.InitializeComponent();
            
            App.Geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            App.Geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

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

        public PlaceViewModel PlaceViewModel
        {
            get { return this._placeViewModel; }
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
            var travelId = (int)e.NavigationParameter;
            _travel = await _travelRepository.GetTravelByIdAsync(travelId);

            _placeViewModel.Data = await _placeRepository.GetParentPlaces(travelId);
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
            App.Geolocator.PositionChanged -= new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            App.Geolocator.StatusChanged -= new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region positioning

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CalculateDistances();
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
                        CalculateDistances();
                        break;

                    case PositionStatus.Disabled:
                        MessageBoxHelper.ShowNoLocation();
                        break;
                    case PositionStatus.Initializing:
                    case PositionStatus.NoData:
                    case PositionStatus.NotInitialized:
                    case PositionStatus.NotAvailable:
                    default:
                        break;

                }
            });
        }

        private async void CalculateDistances()
        {
            var pos = await App.Geolocator.GetGeopositionAsync();

            //delay this
            int i = 0;
            while (_placeViewModel.Data == null || i++<10)
            {
                await Task.Delay(300);
            }

            foreach(var place in _placeViewModel.Data)
            {
                place.SetDistance(pos);
            }
        }

        #endregion

        

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (PlaceViewModel)e.ClickedItem;

            Frame.Navigate(typeof(PlaceMain), clickedItem.Id);

            //TODO navigate to places details

            //HubSection hubSection = null;
            //Add hub item and navigate to it
            //if (MainHub.Sections.Count < 3)
            //{
            //    hubSection = new HubSection();
            //    MainHub.Sections.Add(hubSection);
            //}
            //else //Or modify existing hub
            //{
            //    hubSection = MainHub.Sections[2];
            //}

            //hubSection.Header = clickedItem.Name;
            //hubSection.DataContext = clickedItem;
            //hubSection.ContentTemplate = (DataTemplate)this.Resources["PlaceDescriptionTemplate"];
            //MainHub.ScrollToSection(hubSection);
        }

    }
}
