using VirtualGuide.Mobile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VirtualGuide.Mobile.ViewModel;
using VirtualGuide.Mobile.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VirtualGuide.Mobile.Repository;
using System.Net.Http;
using VirtualGuide.Mobile.Helper;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuideList : Page
    {
        private NavigationHelper navigationHelper;

        private TravelRepository _travelRepository = new TravelRepository();
        private TravelViewModel _travelViewModel = new TravelViewModel();

        public GuideList()
        {
            this.InitializeComponent();

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
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //TODO Check if token is still active
            SetupList();
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
            this.navigationHelper.OnNavigatedFrom(e);

        }

        #endregion

        private void OwnedTravels_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(GuideMain), (e.ClickedItem as TravelViewModel).Id);
        }

        private async void SetupList()
        {
            //Get places from DB
            _travelViewModel.Data = new List<TravelViewModel>(await _travelRepository.GetAllTravelsAsync());
            AllTravelsList.ItemsSource = _travelViewModel.Collection.View;

            //start downloading data
            Task<List<TravelViewModel>> availableTravelsTask = _travelRepository.GetAvailableTravels();
            
            //Download available places
            //TODO: download only these not owned by user yet
            try
            {
                var _availableTravels = await availableTravelsTask;
                var availableTravelsList = new List<TravelViewModel>(_availableTravels);

                _travelViewModel.Data.AddRange(availableTravelsList);
                AllTravelsList.ItemsSource = _travelViewModel.Collection.View;
                
            }
            catch (Exception)
            {
                NoConnectionMessage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                Task<List<TravelViewModel>> ownedTravelsTask = _travelRepository.DownloadAndSaveOwnedTravels();

                var _ownedTravels = await ownedTravelsTask;
                ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                
                _travelViewModel.Data = new List<TravelViewModel>(_ownedTravels);
                AllTravelsList.ItemsSource = _travelViewModel.Collection.View;
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == System.Net.HttpStatusCode.NotFound.ToString())
                {
                    MessageBoxHelper.Show("Please check your internet connection.", "No Connection");
                }
                else if (ex.Message == System.Net.HttpStatusCode.Unauthorized.ToString())
                {
                    MessageBoxHelper.Show("Please log in using your login and password.", "No identity");
                    this.Frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    MessageBoxHelper.Show("Unexpected error occured. Please try again later.", "Error");

                }
            }
        }
    }
}
