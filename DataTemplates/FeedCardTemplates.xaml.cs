using Coolapk_UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Fuck_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            var feed = ((FrameworkElement)sender).DataContext as Feed;
            App.AppViewModel.HomeContentFrame.Navigate(typeof(Pages.UserProfile), feed.UserInfo.Uid);
        }
    }
}
