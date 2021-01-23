using Coolapk_UWP.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Coolapk_UWP.Pages {
    public sealed partial class UserProfile : Page {
        public readonly UserProfileViewModel ViewModel;

        public UserProfile() {
            ViewModel = new UserProfileViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            var parameters = e.Parameter;
            if (parameters != null && parameters is int) {
                ViewModel.Uid = parameters as int?;
            }
            ViewModel.Reload();
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e) {
            ViewModel.Reload();
        }

        private void Pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args) {
            ViewModel.OnPiovtSelect(args.Item.Content.ToString());
        }
    }
}
