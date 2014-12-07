using System.Linq;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.ViewModel.GuideMain;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuideMain : Page
    {
        private GuideMainViewModel viewModel;

        /// TODO: Create your own Hub control based on Hub
        public GuideMain()
        {
            this.InitializeComponent();

            viewModel = DataContext as GuideMainViewModel;
            viewModel.SetNavigationHelper(new NavigationHelper(this));    

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;


            viewModel.DataLoaded += CreateHubSections;
            viewModel.ScrollRequested += ScrollToItem;
        }

        #region Navigation

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel.OnNavigatedToCommand.Execute(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            viewModel.OnNavigatedFromCommand.Execute(e);

        }

        #endregion

        #region Event handling
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (!MainHub.SectionsInView.First().Equals(MainHub.Sections.First()))
            {
                e.Handled = true;
                //TODO ANIMATE me
                MainHub.ScrollToSection(MainHub.Sections.First());
            }
        }

        private void ScrollToItem(PropertyBindingModel item)
        {
            if (item == null) return;

            var hubElement = MainHub.Sections.Where(x => x.DataContext is PropertyBindingModel && ((PropertyBindingModel)x.DataContext).Id == item.Id).First();
            UIHelper.ScollHubToSection(hubElement, ref MainHub);
        }

        private void CreateHubSections()
        {
            foreach (var property in viewModel.Properties)
            {
                if (property.Type != PropertyBindingModel.Types.REGULAR)
                    continue;

                HubSection hubSection = new HubSection();
                hubSection.DataContext = property;
                hubSection.ContentTemplate = (DataTemplate)Application.Current.Resources["PropertyContentTemplate"];
                hubSection.HeaderTemplate = (DataTemplate)Application.Current.Resources["PropertyHeaderTemplate"];

                MainHub.Sections.Add(hubSection);
            }
        }

        #endregion
    }
}
