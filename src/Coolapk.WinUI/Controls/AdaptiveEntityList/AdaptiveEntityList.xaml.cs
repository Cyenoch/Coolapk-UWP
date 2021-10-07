﻿using Coolapk.ViewModels.Controls;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Coolapk.WinUI.Controls.AdaptiveEntityList
{
    public partial class AdaptiveEntityList : UserControl, IViewFor<AdaptiveEntityListViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(AdaptiveEntityListViewModel), typeof(AdaptiveEntityList), new PropertyMetadata(null));

        public AdaptiveEntityList()
        {
            InitializeComponent();
            _ = this.WhenActivated(disposable => {
                
            });
        }

        public AdaptiveEntityListViewModel ViewModel { get => (AdaptiveEntityListViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }
        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (AdaptiveEntityListViewModel)value; }
    }
}