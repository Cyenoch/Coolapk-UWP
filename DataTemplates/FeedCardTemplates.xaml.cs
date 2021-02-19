using Coolapk_UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.DataTemplates {

    public class FeedCardTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            Feed feed = (Feed)item;
            return DefaultTemplate;
        }
    }

    public partial class FeedCardTemplates : ResourceDictionary {

        public FeedCardTemplates() {
            this.InitializeComponent();
        }

        private void UserProfileBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            var feed = ((FrameworkElement)sender).DataContext as Feed;
            App.AppViewModel.HomeContentFrame.Navigate(typeof(Pages.UserProfile), feed.UserInfo.Uid);
            e.Handled = true;
        }

        private void ForwardFeedBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            var feed = (sender as FrameworkElement).DataContext as Feed;
            App.AppViewModel.HomeContentFrame.Navigate(typeof(Coolapk_UWP.Pages.FeedDetail), feed.ForwardSourceFeed.EntityID);
            e.Handled = true;
        }

        private void FeedBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            var feed = (sender as FrameworkElement).DataContext as Feed;
            App.AppViewModel.HomeContentFrame.Navigate(typeof(Coolapk_UWP.Pages.FeedDetail), feed.EntityID);
            e.Handled = true;
        }

        private async void LikeButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            e.Handled = true;
            var feed = (sender as FrameworkElement).DataContext as Feed;
            try {
                _ = await feed.ToggleLike();
            } catch (Exception err) {
                var dialog = new ContentDialog() {
                    PrimaryButtonText = "确定",
                    Content = err.Message,
                    Title = "点赞失败",
                };
                _ = dialog.ShowAsync();
            }
        }

        private void FeedCardMore_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            e.Handled = true;
            var feed = (sender as FrameworkElement).DataContext as Feed;
        }

        private void RelationRows_ItemClick(object sender, ItemClickEventArgs e) {

        }
    }
}
