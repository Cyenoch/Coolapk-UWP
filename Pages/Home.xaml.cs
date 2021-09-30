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

namespace Coolapk_UWP.Pages
{
    public sealed partial class Home : Page
    {
        private HomeMenuItem CurrentMenuItem;
        Microsoft.UI.Xaml.Controls.NavigationView HomeNavigationView;

        public Home()
        {
            this.InitializeComponent();
            ((HomeViewModel)DataContext).Reload();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            Window.Current.Activated += Current_Activated;
        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            SolidColorBrush defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            SolidColorBrush inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];
            AppTitle.Foreground = e.WindowActivationState == CoreWindowActivationState.Deactivated ? inactiveForegroundBrush : defaultForegroundBrush;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            AppTitleBar.Height = coreTitleBar.Height;
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        public void AsyncLoadStateControl_Retry(object sender, RoutedEventArgs a)
        {
            ((HomeViewModel)DataContext).Reload();
        }

        private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            const int topIndent = 16;
            const int expandedIndent = 48;
            int minimalIndent = 104;
            // 如果返回按钮未显示，则削减TitleBar的空间
            if (HomeNavigationView == null) return;
            if (HomeNavigationView.IsBackButtonVisible.Equals(Microsoft.UI.Xaml.Controls.NavigationViewBackButtonVisible.Collapsed))
            {
                minimalIndent = 48;
            }

            Thickness currMargin = AppTitleBar.Margin;

            if (sender.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top)
            {
                // 如果是Top Mode
                AppTitleBar.Margin = new Thickness(topIndent, currMargin.Top, currMargin.Left, currMargin.Right);
            }
            else if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
            {
                // 如果是 minimal
                AppTitleBar.Margin = new Thickness(minimalIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            else
            {
                AppTitleBar.Margin = new Thickness(expandedIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
        }

        private void ContentFrameControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppViewModel.HomeContentFrame = sender as Frame;
            if ((sender as Frame).BackStackDepth == 0) App.AppViewModel.HomeContentFrame.Navigate(typeof(LaunchPad));

        }

        private void NavigationViewControl_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            if (App.AppViewModel.HomeContentFrame.CanGoBack) App.AppViewModel.HomeContentFrame.GoBack();
        }

        private void NavigationViewControl_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
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
                    var frame = App.AppViewModel.HomeContentFrame;
                    MainInitTabConfig target;
                    if (menuItem.Children != null && menuItem.Children.Count > 0 && CurrentMenuItem == null)
                    {
                        target = menuItem.DefaultConfig ?? menuItem.Children[0].Config;
                        var targetItem = menuItem.Children.First(child => child.Config == target);
                        if (targetItem == null) return;
                        _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            sender.SelectedItem = targetItem;
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

        private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            HomeNavigationView = sender as Microsoft.UI.Xaml.Controls.NavigationView;
        }
    }
}
