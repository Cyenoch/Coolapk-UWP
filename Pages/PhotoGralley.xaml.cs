using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Coolapk_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PhotoGralley : Page
    {
        public PhotoGralley()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(AppTitleBar);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is IList<string> picArr)
            {
                ViewModel.Photos = picArr;
            }
        }


        public static void Navigate(IList<string> PicArr)
        {
            (Window.Current.Content as Frame).Navigate(typeof(PhotoGralley), PicArr);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).GoBack();
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            // TODO:
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var current = ViewModel.Photos[PhotosFlip.SelectedIndex];
                // TODO: do sth
            } catch(Exception err)
            {

            }
        }
    }
}
