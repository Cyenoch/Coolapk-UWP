using Coolapk_UWP.Other;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http.Filters;

namespace Coolapk_UWP.Pages {
    public sealed partial class Login : Page {
        public Login() {
            this.InitializeComponent();
            //WebView
        }

        private async void LoginWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args) {
            if (args.Uri.OriginalString == "https://www.coolapk.com/") {
                var dialog = new ContentDialog {
                    Title = "是否登录完成",
                    Content = "检测到你可能已经完成登录",
                    IsPrimaryButtonEnabled = true,
                    IsSecondaryButtonEnabled = true,
                    PrimaryButtonText = "验证状态",
                    SecondaryButtonText = "重试"
                };
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                    AppBarButton_Tapped(null, null);
                else if (result == ContentDialogResult.Secondary)
                    LoginWebView.Navigate(new Uri("https://account.coolapk.com/"));

            }
        }

        private void AppBarButtonClearLoginState_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            App.AppViewModel.Logout();
            LoginWebView.Navigate(new Uri("https://account.coolapk.com/"));
        }

        private void verify() {
            var dialog = new ContentDialog {
                Title = "验证中...",
                IsPrimaryButtonEnabled = false,
                IsSecondaryButtonEnabled = false
            };
            var dialogResult = dialog.ShowAsync();

            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => {
                try {
                    App.AppViewModel.LoadLoginState(); // if failed throw exception
                    dialog.Title = "验证成功";
                    dialog.PrimaryButtonText = "点击左上角返回键返回";
                    dialog.IsPrimaryButtonEnabled = true;
                } catch (Exception ___) {
                    dialog.Title = "验证失败";
                    if (___ is NullReferenceException) dialog.Content = "未获取到uid cookie";
                    else dialog.Content = ___.Message;
                    dialog.SecondaryButtonText = "重试";
                    dialog.SecondaryButtonClick += Dialog_PrimaryButtonClick;
                    dialog.IsSecondaryButtonEnabled = true;
                }
            });
        }

        private void AppBarButton_Tapped(object _, Windows.UI.Xaml.Input.TappedRoutedEventArgs __) {
            verify();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            LoginWebView.Navigate(new Uri("https://account.coolapk.com/"));
        }
    }
}
