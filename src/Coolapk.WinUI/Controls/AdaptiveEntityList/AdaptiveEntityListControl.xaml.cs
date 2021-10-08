using ReactiveUI;
using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Coolapk.WinUI.Controls.AdaptiveEntityList
{
    public partial class AdaptiveEntityListControl : UserControl, IViewFor<AdaptiveEntityListViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(AdaptiveEntityListViewModel), typeof(AdaptiveEntityListControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ConfigProperty = DependencyProperty
            .Register(nameof(Config), typeof(AdaptiveEntityListViewModel), typeof(AdaptiveEntityListConfig), new PropertyMetadata(null));
        public AdaptiveEntityListControl() : this(null) { }
        public AdaptiveEntityListControl(AdaptiveEntityListConfig Config = null)
        {
            if (Config != null) this.Config = Config;
            if (ViewModel == null) ViewModel = new AdaptiveEntityListViewModel(vm => Config.Fetcher(this, vm, Config));
            InitializeComponent();
            _ = this.WhenActivated(disposable =>
            {
                if (Config != null && ViewModel.IsInitializeLoading) _ = ViewModel.InitializeRequestAsync();

                _ = this.OneWayBind(ViewModel, vm => vm.Data.Count, v => v.TextA.Text)
                    .DisposeWith(disposable);

                _ = this.OneWayBind(ViewModel, vm => vm.ErrorText, v => v.ErrorA.Text)
                    .DisposeWith(disposable);

                //_ = this.OneWayBind(ViewModel, vm => vm.Data, v => v.DataListControl.ItemsSource)
                //    .DisposeWith(disposable);
            });
            var tmplSelector = DataListControl.ItemTemplateSelector as AdaptiveItemTemplateSelector;
        }

        public AdaptiveEntityListConfig Config { get => (AdaptiveEntityListConfig)GetValue(ConfigProperty); set => SetValue(ConfigProperty, value); }

        public AdaptiveEntityListViewModel ViewModel { get => (AdaptiveEntityListViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }
        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (AdaptiveEntityListViewModel)value; }
    }
}
