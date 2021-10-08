using Coolapk.Models;
using Coolapk.ViewModels.Home;
using Coolapk.WinUI.Controls.AdaptiveEntityList;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Coolapk.WinUI.Pages
{
    public sealed partial class HomePage : Page, IViewFor<HomeViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(HomeViewModel), typeof(HomePage), new PropertyMetadata(null));

        public HomePage()
        {
            InitializeComponent();
            ViewModel = new HomeViewModel();
            _ = this.WhenActivated(disposable =>
              {
                  _ = this.OneWayBind(ViewModel, vm => vm.MenuItems, v => v.NavigationViewControl.MenuItemsSource)
                      .DisposeWith(disposable);

                  //this.BindCommand(ViewModel, vm => vm.ChangeTitleCommand, v => v.ChangeTitleButton)
                  //    .DisposeWith(disposable);

                  if (!ViewModel.MenuItems.Any()) _ = ViewModel.InitializeRequestAsync();

              });

            SetupTitleBar();
        }

        public HomeViewModel ViewModel { get => (HomeViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }
        object IViewFor.ViewModel
        {
            get => ViewModel; set => ViewModel = (HomeViewModel)value;
        }
        // 最后导航到的item
        private SelectableItem LastSelectedItem;

        private void NavigationViewControl_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrameControl.CanGoBack) ContentFrameControl.GoBack();
        }

        private void SearchInput_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void ContentFrameControl_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (e.SourcePageType == typeof(SettingsPage))
                {
                    NavigationViewControl.SelectedItem = NavigationViewControl.SettingsItem;
                }
                else if (e.SourcePageType == typeof(AdaptiveEntityListWrapperPage))
                {
                    var target = ViewModel.MenuItems.First(item => item.Title == (e.Parameter as AdaptiveEntityListConfig).Title);
                    NavigationViewControl.SelectedItem = target;
                    LastSelectedItem = target as SelectableItem;
                }
            }
        }

        private void NavigationViewControl_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (!args.IsSettingsInvoked)
            {
                var selected = ViewModel.MenuItems.First(item => item.Title == (string)args.InvokedItem);
                NavigationViewInvoked(selected, args.IsSettingsInvoked);
            }
            else
            {
                NavigationViewInvoked(null, true);
            }
        }

        private void NavigationViewInvoked(MenuItem selected, bool isSettingsInvoked = false)
        {
            try
            {
                switch (selected)
                {
                    case SelectableItem item:
                        // 如果最后选择的item就是当前要导航到的item则忽略
                        if (LastSelectedItem == item) break;
                        _ = ContentFrameControl.Navigate(typeof(AdaptiveEntityListWrapperPage), new AdaptiveEntityListConfig()
                        {
                            Title = item.Config.Title,
                            Url = item.Config.Url,
                            Fetcher = async (control, vm, config) =>
                            {
                                var resp = (await vm.ApiService.GetDataList(config.Url, config.Title, vm.Page));
                                if (resp.Error != null) throw new Exception(resp.Error);
                                return resp.Data;
                            },
                        });
                        LastSelectedItem = item;
                        break;
                    //case MultiChildItem item:
                    //    var targetItem = item.SubItem.ToList()[item.DefaultSelect];
                    //    if (LastSelectedItem == targetItem) break;
                    //    NavigationViewControl.SelectedItem = targetItem;
                    //    //NavigationViewInvoked(targetItem);
                    //    break;
                    default:
                        if (isSettingsInvoked && ContentFrameControl.CurrentSourcePageType != typeof(SettingsPage))
                        {
                            _ = ContentFrameControl.Navigate(typeof(SettingsPage));
                            LastSelectedItem = null;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                //
            }
        }
    }

    // 为了自定义TitleBar所作的操作
    public partial class HomePage
    {
        private void SetupTitleBar()
        {
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

        private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            const int topIndent = 16;
            const int expandedIndent = 48;
            int minimalIndent = 104;
            // 如果返回按钮未显示，则削减TitleBar的空间
            if (NavigationViewControl == null) return;
            if (NavigationViewControl.IsBackButtonVisible.Equals(Microsoft.UI.Xaml.Controls.NavigationViewBackButtonVisible.Collapsed))
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
            //AppTitleBar.Height = coreTitleBar.Height;
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }
    }
}
