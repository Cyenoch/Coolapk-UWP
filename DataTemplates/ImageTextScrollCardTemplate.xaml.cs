using Coolapk_UWP.Pages;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Coolapk_UWP.DataTemplates {
    public sealed partial class ImageTextScrollCardTemplate : ResourceDictionary {
        public ImageTextScrollCardTemplate() {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
            var entity = e.ClickedItem as Coolapk_UWP.Models.Entity;
            if (entity is Coolapk_UWP.Models.Feed) {
                App.AppViewModel.HomeContentFrame.Navigate(typeof(FeedDetail), entity.EntityID);
            }
        }
    }
}
