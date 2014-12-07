using System;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.ViewModel.MapPage;
using Windows.Devices.Geolocation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MapPage : Page
    {
        private MapViewModel viewModel;

        public MapPage()
        {
            this.InitializeComponent();

            viewModel = DataContext as MapViewModel;
            viewModel.SetNavigationHelper(new NavigationHelper(this)); 
            viewModel.ZoomingMapToPoint += ViewModel_ZoomingMapToPoint;

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;


            
        }

        private async void ViewModel_ZoomingMapToPoint(Geopoint center, double zoomLevel)
        {
            await Maps.TrySetViewAsync(center, zoomLevel, null, null, MapAnimationKind.Default);
        }

        #region Navigation

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel.OnNavigatedToCommand.Execute(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            viewModel.OnNavigatedFromCommand.Execute(e);

        }

        #endregion

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
    }

}
