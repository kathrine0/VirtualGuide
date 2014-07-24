using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

        private List<SimplePropertyViewModel> _propertiesList = new List<SimplePropertyViewModel>() { 
            new SimplePropertyViewModel() {Name = "Maps and places", Background=VirtualGuide.Mobile.Common.ColorHelper.BLUE, Symbol="\uD83C\uDF0D", Type=SimplePropertyViewModel.Types.MAPS},
            new SimplePropertyViewModel() {Name = "Tours", Background=VirtualGuide.Mobile.Common.ColorHelper.GREEN, Symbol="\uD83C\uDFF0", Type=SimplePropertyViewModel.Types.TOURS},
        };
        private PropertyViewModel _propertyViewModel = new PropertyViewModel();

        private int _travelId;

        public GuideMain()
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            _travelId = (int) e.Parameter;

            var dbProps = await _propertyViewModel.GetSimpleProperties(_travelId);

            _propertiesList.AddRange(dbProps);
            PropertiesView.ItemsSource = _propertiesList;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

        }

        private void PropertiesView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (SimplePropertyViewModel) e.ClickedItem;

            switch(clickedItem.Type)
            {
                case SimplePropertyViewModel.Types.MAPS:
                    Frame.Navigate(typeof(GuideMaps), _travelId);
                break;
                case SimplePropertyViewModel.Types.TOURS:
                break;
                case SimplePropertyViewModel.Types.REGULAR:
                break;
            }
        }

        
    }
}
