using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Helper;
using Windows.UI.Xaml.Navigation;

namespace VirtualGuide.Mobile.ViewModel.Interfaces
{
    public interface INavigableViewModel
    {
        RelayCommand<NavigationEventArgs> OnNavigatedToCommand { get; set; }
        RelayCommand<NavigationEventArgs> OnNavigatedFromCommand { get; set; }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        NavigationHelper NavigationHelper { get; }

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
        void NavigationHelper_LoadState(object sender, LoadStateEventArgs e);

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        void NavigationHelper_SaveState(object sender, SaveStateEventArgs e);

        void SetNavigationHelper(NavigationHelper nh);
    }
}
