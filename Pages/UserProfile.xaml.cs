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
            var parameter = e.Parameter;
            if (parameter != null) {
                switch (parameter) {
                    case uint _uintUid:
                    case int _intUid:
                        var uid = (uint)parameter;
                        if (uid != ViewModel.Uid) {
                            ViewModel.Uid = uid;
                            ViewModel.Reload();
                        }
                        break;
                    case string username:
                        if (username != ViewModel.Username) {
                            ViewModel.Username = username;
                            ViewModel.Reload();
                        }
                        break;
                }
            }
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e) {
            ViewModel.Reload();
        }

        private void Pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args) {
            ViewModel.OnPiovtSelect(args.Item.Content.ToString());
        }
    }
}
