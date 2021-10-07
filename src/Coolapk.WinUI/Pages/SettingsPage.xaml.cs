using Coolapk.WinUI.ViewModels;
using ReactiveUI;
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

namespace Coolapk.WinUI.Pages
{
    public sealed partial class SettingsPage : Page, IViewFor<SettingsPageViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(SettingsPageViewModel), typeof(SettingsPage), new PropertyMetadata(null));

        public SettingsPage()
        {
            InitializeComponent();
        }

        public SettingsPageViewModel ViewModel { get => (SettingsPageViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }
        object IViewFor.ViewModel
        {
            get => ViewModel; set => ViewModel = (SettingsPageViewModel)value;
        }
    }
}
