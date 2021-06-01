using Coolapk_UWP.Models;
using Coolapk_UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Coolapk_UWP.Pages
{
    public sealed partial class Home : Page
    {

        HomeMenuItem CurrentMenuItem;
        Microsoft.UI.Xaml.Controls.NavigationView HomeNavigationView;

        public Home()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            ((HomeViewModel)DataContext).Reload();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            AppTitleBar.Height = coreTitleBar.Height;
            App.AppViewModel.AppBarHeight = coreTitleBar.Height;

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonHoverBackgroundColor = Color.FromArgb(40, 0, 0, 0);
            viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonPressedBackgroundColor = Color.FromArgb(90, 0, 0, 0);
            viewTitleBar.ButtonForegroundColor = ((SolidColorBrush)Resources["ISystemBaseHighColor"]).Color;

            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
            displayInformation.DpiChanged += DisplayProperties_DpiChanged;

            Window.Current.SetTitleBar(AppTitleBar);
        }

        public void AsyncLoadStateControl_Retry(object sender, RoutedEventArgs a)
        {
            ((HomeViewModel)DataContext).Reload();
        }

        private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            HomeNavigationView = sender;
            switch (args.SelectedItem)
            {
                case SpecialHomeMenuItem specialItem:
                    switch (specialItem.Tag)
                    {
                        case "发布动态":
                            App.AppViewModel.HomeContentFrame.Navigate(typeof(Pages.CreateFeed));
                            break;
                    }
                    CurrentMenuItem = specialItem;
                    break;
                case NavigationViewItem viewItem:
                    switch (viewItem.Content)
                    {
                        case "设置":
                            break;
                    }
                    break;
                case HomeMenuItem menuItem:
                    var frame = sender.Content as Frame;
                    MainInitTabConfig target;
                    if (menuItem.Children != null && menuItem.Children.Count > 0)
                    {
                        target = menuItem.DefaultConfig ?? menuItem.Children[0].Config;
                        _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            sender.SelectedItem = menuItem.Children.First(child => child.Config == target);
                        });
                    }
                    else
                    { // 最终目标tab
                        target = menuItem.Config;
                        if (CurrentMenuItem != menuItem)
                            frame.Navigate(typeof(DataListWrapper), menuItem);
                        CurrentMenuItem = menuItem;
                    }
                    break;
            }
        }

        private void AppRootFrame_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppViewModel.AppRootFrame = sender as Frame;
        }

        private void ContentFrame_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppViewModel.HomeContentFrame = sender as Frame;
            if (App.AppViewModel.HomeContentFrame.BackStackDepth == 0) App.AppViewModel.HomeContentFrame.Navigate(typeof(LaunchPad));
        }

        private void HomeNavigationView_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            if (App.AppViewModel.AppRootFrame.CanGoBack) App.AppViewModel.AppRootFrame.GoBack();
            else if (App.AppViewModel.HomeContentFrame.CanGoBack) App.AppViewModel.HomeContentFrame.GoBack();
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitleBar.Height = sender.Height;
            App.AppViewModel.AppBarHeight = sender.Height;
        }

        // 细品
        private void HomeNavigationView_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            if (sender.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top)
            {
                AppRootFrame.Padding = new Thickness { Top = AppTitleBar.Height };
                AppTitleBar.Margin = new Thickness { Left = 0 };
            }
            else
            {
                AppRootFrame.Padding = new Thickness { Top = 0 };
                AppTitleBar.Margin = new Thickness { Left = sender.CompactPaneLength };
            }
        }

        private void HomeNavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            HomeNavigationView = sender as Microsoft.UI.Xaml.Controls.NavigationView;
        }

        private void DisplayProperties_DpiChanged(DisplayInformation info, object o)
        {

        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            HomeNavigationView.IsBackEnabled = ((Frame)sender).CanGoBack;
        }

        private void AppRootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            HomeNavigationView.IsBackEnabled = ((Frame)sender).CanGoBack;
        }
    }
}
