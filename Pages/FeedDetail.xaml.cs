using Coolapk_UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Coolapk_UWP.Pages {
    public sealed partial class FeedDetail : Page {
        public FeedDetailViewModel ViewModel => DataContext as FeedDetailViewModel;
        public FeedDetail() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var param = e.Parameter;
            if (param == null) {
                ViewModel.ErrorMessage = "请传入正确的参数";
                return;
            } else if (ViewModel.Data == null) {
                ViewModel.FeedId = (uint)param;
                ViewModel.Reload();
            }
        }

        private void AsyncLoadStateControl_Retry(object sender, RoutedEventArgs e) {
            ViewModel.Reload();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e) {
            App.AppViewModel.HomeContentFrame.Navigate(typeof(UserProfile), ViewModel.Data.UserInfo.Uid);
        }

        private void FollowButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
