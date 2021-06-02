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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Coolapk_UWP.Pages
{
    public sealed partial class LaunchPad : Page
    {
        public AppViewModel ViewModel
        {
            get { return App.AppViewModel; }
        }
        public LaunchPad()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        private void LaunchPadListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch ((e.ClickedItem as FrameworkElement).Name)
            {
                case "GoToLogin":
                    App.AppViewModel.HomeContentFrame.Navigate(typeof(Login));
                    break;
                case "GoToGitHub":
                    var uri = new Uri(@"https://github.com/Cyenoch/Coolapk-UWP");
                    Windows.System.Launcher.LaunchUriAsync(uri);
                    break;
                case "GoToUserProfile":
                    break;
                case "FollowDeveloper":
                    break;
                case "GoToQQGroup":
                    Windows.System.Launcher.LaunchUriAsync(new Uri(@"https://jq.qq.com/?_wv=1027&k=0Uvdqt1d"));
                    break;
            }
        }
    }
}
