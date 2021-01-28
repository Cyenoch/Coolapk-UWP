using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Coolapk_UWP.Pages;
using Coolapk_UWP.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace Coolapk_UWP {
    sealed partial class App : Application {
        public static AppViewModel AppViewModel;

        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            App.AppViewModel = new AppViewModel();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {

            //var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //coreTitleBar.ExtendViewIntoTitleBar = true;

            //var view = ApplicationView.GetForCurrentView();

            //// active
            //view.TitleBar.BackgroundColor = Color.FromArgb(255, 8, 87, 180);
            //view.TitleBar.ForegroundColor = Colors.White;

            //// inactive  
            //view.TitleBar.InactiveBackgroundColor = Color.FromArgb(255, 8, 87, 180);
            //view.TitleBar.InactiveForegroundColor = Colors.Black;

            //// button
            //view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 8, 87, 180);
            //view.TitleBar.ButtonForegroundColor = Colors.White;

            //view.TitleBar.ButtonHoverBackgroundColor = Colors.Blue;
            //view.TitleBar.ButtonHoverForegroundColor = Colors.White;

            //view.TitleBar.ButtonPressedBackgroundColor = Colors.Blue;
            //view.TitleBar.ButtonPressedForegroundColor = Colors.White;

            //view.TitleBar.ButtonInactiveBackgroundColor = Colors.DarkGray;
            //view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    rootFrame.Navigate(typeof(Home), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}