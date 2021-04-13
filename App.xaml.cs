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
using System.Threading.Tasks;
using Coolapk_UWP.Other;

namespace Coolapk_UWP {
    sealed partial class App : Application {
        public static AppViewModel AppViewModel;

        public App() {
            this.InitializeComponent();
            EmojisUtil.LoadEmojisResw();
            this.Suspending += OnSuspending;
            App.AppViewModel = new AppViewModel();
        }

        private void HandleActivation(IActivatedEventArgs e) {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            var launch = e as LaunchActivatedEventArgs;
            if (launch != null && launch.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    rootFrame.Navigate(typeof(Home), launch.Arguments);
                }
                Window.Current.Activate();
            } else {
                if (rootFrame.Content == null) {
                    rootFrame.Navigate(typeof(Home));
                }
                Window.Current.Activate();
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            HandleActivation(e);
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args) {
            base.OnActivated(args);
            HandleActivation(args);

            if (args.Kind == ActivationKind.Protocol)
                switch (args) {
                    case ProtocolActivatedEventArgs args1:
                        if (args1.Uri.Scheme == "coolmarket") {
                            switch (args1.Uri.Host) {
                                case "feed": // 动态
                                    var feedId = args1.Uri.Segments[1];
                                    if (feedId != null) {
                                        // 跳转到动态
                                        if (AppViewModel.HomeContentFrame == null) App.AppViewModel.HomeContentFrameLoadedEvent += new AppViewModel.HomeContentFrameLoadedHandler(async homeContentFrame => {
                                            await Task.Delay(200);
                                            homeContentFrame.Navigate(typeof(Coolapk_UWP.Pages.FeedDetail), uint.Parse(feedId));
                                        });
                                        else AppViewModel.HomeContentFrame.Navigate(typeof(Coolapk_UWP.Pages.FeedDetail), uint.Parse(feedId));
                                    }
                                    break;
                            }
                        }
                        break;
                }
        }
    }
}