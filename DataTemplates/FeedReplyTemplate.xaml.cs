using Coolapk_UWP.Models;
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

namespace Coolapk_UWP.DataTemplates {
    public sealed partial class FeedReplyTemplate : ResourceDictionary {
        public FeedReplyTemplate() {
            this.InitializeComponent();
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e) {
            var model = (FeedReply)((FrameworkElement)sender).DataContext;
            model.Likenum += 1;
        }
    }
}
