﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.ViewModel.Interfaces;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VirtualGuide.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private INavigableViewModel viewModel;


        public RegisterPage()
        {
            this.InitializeComponent();

            viewModel = DataContext as INavigableViewModel;
            viewModel.SetNavigationHelper(new NavigationHelper(this));

            this.SpinningAnimation.Begin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel.OnNavigatedToCommand.Execute(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            viewModel.OnNavigatedFromCommand.Execute(e);

        }
    }
}
