using Coolapk.ViewModels.Home;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
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

namespace Coolapk.WinUI.Pages
{
    public sealed partial class HomePage : Page, IViewFor<HomeViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(HomeViewModel), typeof(HomePage), new PropertyMetadata(null));

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel();
            
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.TitleBlock.Text)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.ChangeTitleCommand, v => v.ChangeTitleButton)
                    .DisposeWith(disposable);
            });
        }

        public HomeViewModel ViewModel { get => (HomeViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }
        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (HomeViewModel)value; }
    }
}
