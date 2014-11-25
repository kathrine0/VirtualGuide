using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.ViewModel.GuideList;
using VirtualGuide.Mobile.ViewModel.GuideMain;
using VirtualGuide.Mobile.ViewModel.LoginPage;
using VirtualGuide.Mobile.ViewModel.MapPage;
using VirtualGuide.Mobile.ViewModel.PlaceMain;
using VirtualGuide.Mobile.ViewModel.RegisterPage;
using Windows.UI.Xaml;

namespace VirtualGuide.Mobile.ViewModel
{
    class ViewModelLocator
    {
        #region constructors

        static ViewModelLocator()
        {          
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService>(() => CreateNavigationService());
            
            SimpleIoc.Default.Register<GuideListViewModel>();
            SimpleIoc.Default.Register<GuideMainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MapViewModel>();
            SimpleIoc.Default.Register<PlaceMainViewModel>();
            SimpleIoc.Default.Register<RegisterViewModel>();
            SimpleIoc.Default.Register<BuyGuideViewModel>();
        }

        #endregion

        #region ViewModels

        public GuideListViewModel GuideList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GuideListViewModel>();
            }
        }
        public GuideMainViewModel GuideMain
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GuideMainViewModel>();
            }
        }
        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public MapViewModel Map
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MapViewModel>();
            }
        }
        public PlaceMainViewModel PlaceMain
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlaceMainViewModel>();
            }
        }
        public RegisterViewModel Register
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterViewModel>();
            }
        }
        public BuyGuideViewModel BuyGuide
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BuyGuideViewModel>();
            }
        }

        #endregion

        #region navigation

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            navigationService.Configure("Login", typeof(VirtualGuide.Mobile.View.LoginPage));
            navigationService.Configure("GuideMain", typeof(VirtualGuide.Mobile.View.GuideMain));
            navigationService.Configure("GuideList", typeof(VirtualGuide.Mobile.View.GuideList));
            navigationService.Configure("Maps", typeof(VirtualGuide.Mobile.View.MapPage));
            navigationService.Configure("Register", typeof(VirtualGuide.Mobile.View.RegisterPage));
            navigationService.Configure("PlaceMain", typeof(VirtualGuide.Mobile.View.PlaceMain));
            navigationService.Configure("BuyGuide", typeof(VirtualGuide.Mobile.View.BuyGuide));

            return navigationService;
        }

        #endregion

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public static void Cleanup()
        {
            var viewModelLocator = (ViewModelLocator)Application.Current.Resources["Locator"];
            viewModelLocator.GuideList.Cleanup();
            viewModelLocator.Login.Cleanup();
            viewModelLocator.Map.Cleanup();
            viewModelLocator.PlaceMain.Cleanup();
            viewModelLocator.Register.Cleanup();
            viewModelLocator.BuyGuide.Cleanup();

            //Messenger.Reset();
        }


    }
}
