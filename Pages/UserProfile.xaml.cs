using Coolapk_UWP.Other;
using Coolapk_UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Coolapk_UWP.Pages {
    public sealed partial class UserProfile : Page {
        public readonly UserProfileViewModel ViewModel;

        public UserProfile() {
            ViewModel = new UserProfileViewModel();
            DataContext = ViewModel;
            //ViewModel.FeedList = new IncrementalLoadingCollection<string>(async count => {
            //    return new Collection<string>() {
            //         Pivot.SelectedItem.ToString(),
            //         Pivot.SelectedItem.ToString(),
            //         Pivot.SelectedItem.ToString(),
            //         Pivot.SelectedItem.ToString(),
            //         Pivot.SelectedItem.ToString(),
            //    };
            //});
            this.InitializeComponent();
        }

        private void Pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args) {
            //ViewModel.ClearFeedList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            var parameters = e.Parameter;
            //if (parameters != null) {
            //    var messageDialog = new MessageDialog($"param is {parameters}");
            //    _ = messageDialog.ShowAsync();
            //} else {

            //    var messageDialog = new MessageDialog($"no params");
            //    _ = messageDialog.ShowAsync();
            //}
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e) {
            ViewModel.Reload();
        }
    }
}
