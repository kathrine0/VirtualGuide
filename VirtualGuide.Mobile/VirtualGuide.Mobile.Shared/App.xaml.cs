﻿using SQLite;
using System;
using System.Diagnostics;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.View;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace VirtualGuide.Mobile
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

#if DEBUG
        public static Stopwatch StopWatch;
#endif

        public const string WebService = @"http://catherine.cloudapp.net:8080/";
        //public const string WebService = @"http://192.168.10.180/VirtualGuide/";
        public static SQLiteAsyncConnection Connection = new SQLiteAsyncConnection("VirtualGuide.db");
        public static ResourceLoader ResLoader = new ResourceLoader();
               
        private LocalDataHelper localDataHelper = new LocalDataHelper();
        private SettingsDataHelper settingsDataHelper = new SettingsDataHelper();

        
        public static string MapToken = "6lLX1mlgjcbABymCCQ-y2w";
        public static string GmapsToken = "AIzaSyBNBqnWyCp2fz0gSKTTK_n7uYQPxTdCQ_0";
        //public static string GmapsToken = "AtxbEfXDApZcBF2d2ngikWZxfUBuIpz82WF6btHwrts4Vx8_R_kvJFxw6YvXgt-a";

        public static Frame RootFrame;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
                StopWatch = new Stopwatch();
                StopWatch.Start();
            }
#endif
            // Setup database
            SetupDatabase();

            // Setup automapper
            AutoMapperConfig.Configure();

            //Remove old runtime data
            localDataHelper.DeleteContainer();
            //Create runtime data container
            localDataHelper.CreateContainer();


            RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                RootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            if (RootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter                    
                var page = typeof(LoginPage);
                if (!String.IsNullOrEmpty(settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN)))
                {
                    page = typeof(GuideList);
                }

                if (!RootFrame.Navigate(page, e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

           

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void SetupDatabase()
        {
            //await Connection.DropTableAsync<Travel>();
            //create sqlite database tables
            await Connection.CreateTableAsync<User>();
            await Connection.CreateTableAsync<Travel>();
            await Connection.CreateTableAsync<Property>();
            await Connection.CreateTableAsync<Place>();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            SQLite.SQLiteConnectionPool.Shared.Reset();
            
            //Remove runtime data
            localDataHelper.DeleteContainer();

            deferral.Complete();
        }
    }
}