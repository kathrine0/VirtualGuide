using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Repository;
using VirtualGuide.Mobile.ViewModel;
using Windows.Devices.Geolocation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuideMain : Page
    {
        private NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        private List<PropertyViewModel> _propertiesListAll = new List<PropertyViewModel>() { 
            new PropertyViewModel() {Name = "Places", Background=VirtualGuide.Mobile.Helper.ColorHelper.BLUE, Symbol="\uD83C\uDF0D", Type=PropertyViewModel.Types.MAPS},
            new PropertyViewModel() {Name = "Tours", Background=VirtualGuide.Mobile.Helper.ColorHelper.GREEN, Symbol="\uD83C\uDFF0", Type=PropertyViewModel.Types.TOURS},
        };

        private List<PropertyViewModel> _propertiesList;

        private PropertyRepository _propertyRepository = new PropertyRepository();
        private TravelRepository _travelRepository = new TravelRepository();

        private GuideMainViewModel _travel;

        public GuideMain()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (!MainHub.SectionsInView.First().Equals(MainHub.Sections.First()))
            {
                e.Handled = true;
                //TODO ANIMATE me
                MainHub.ScrollToSection(MainHub.Sections.First());
            }
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
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

            _propertiesList = await _propertyRepository.GetSimpleProperties(travelId);
            _travel = await _travelRepository.GetTravelByIdAsync<GuideMainViewModel>(travelId);

            _propertiesListAll.AddRange(_propertiesList);
            this.DefaultViewModel["Properties"] = _propertiesListAll;
            this.DefaultViewModel["Title"] = _travel.Name;
            this.DefaultViewModel["MapImage"] = new BitmapImage(new Uri(String.Format("ms-appdata:///local/maps/{0}map.png", _travel.Name)));

            CreateHubSections();
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

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

        }

        private void PropertiesView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (PropertyViewModel) e.ClickedItem;

            switch(clickedItem.Type)
            {
                case PropertyViewModel.Types.MAPS:
                    Frame.Navigate(typeof(GuidePlaces), _travel.Id);
                break;
                case PropertyViewModel.Types.TOURS:
                break;
                case PropertyViewModel.Types.REGULAR:
                    var hubElement = MainHub.Sections.Where(x => x.DataContext is PropertyViewModel && ((PropertyViewModel)x.DataContext).Id == clickedItem.Id).First();
                    UIHelper.ScollHubToSection(hubElement, ref MainHub);
                break;
            }
        }

        #region Helper Methods

        private void CreateHubSections()
        {
            foreach (var property in _propertiesList)
            {
                HubSection hubSection = new HubSection();
                hubSection.Header = property.Symbol + property.Name;
                hubSection.DataContext = property;
                hubSection.ContentTemplate = (DataTemplate) this.Resources["PropertyContentTemplate"];

                MainHub.Sections.Add(hubSection);
            }
        }

        #endregion


        private void MapImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapView), _travel.Id);
        }

    }
}
