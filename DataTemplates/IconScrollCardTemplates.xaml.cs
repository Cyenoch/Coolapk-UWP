using Coolapk_UWP.Models;
using Coolapk_UWP.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.DataTemplates {

    public class IconScrollCardItemTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate UserCardTemplate { get; set; }
        public DataTemplate ApkCardTemplate { get; set; }
        // 不知道为什么flipview的selectTemplateCore方法要用这个
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            switch (item) {
                case User _:
                    return UserCardTemplate;
                case Apk _:
                    return ApkCardTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }

    public partial class IconScrollCardTemplates : ResourceDictionary {
        public IconScrollCardTemplates() {
            this.InitializeComponent();
        }

        private void GoToMore_Click(object sender, RoutedEventArgs e) {

        }

        private void UserCardItem_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {

        }

        private void FlipView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            Entity selectedItem = (sender as FlipView).SelectedItem as Entity;
            var goUrl = selectedItem.Url;
            if (goUrl != null) {
                if (goUrl.StartsWith("/u/")) {
                    (Window.Current.Content as Frame).Navigate(typeof(UserProfile), int.Parse(goUrl.Replace("/u/", "")));
                }
            }
        }
    }
}
