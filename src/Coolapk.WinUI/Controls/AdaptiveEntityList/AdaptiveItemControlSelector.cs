using Coolapk.Models;
using Coolapk.WinUI.Controls.AdaptiveEntityList.ItemControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk.WinUI.Controls.AdaptiveEntityList
{
    public sealed partial class AdaptiveItemControlSelector : UserControl
    {
        public static readonly DependencyProperty AdaptiveListViewModelProperty = DependencyProperty.Register("AdaptiveListViewModel", typeof(AdaptiveEntityListViewModel), typeof(AdaptiveItemControlSelector), new PropertyMetadata(null));
        public static readonly DependencyProperty EntityProperty = DependencyProperty.Register("Entity", typeof(Entity), typeof(AdaptiveItemControlSelector), new PropertyMetadata(null));

        public AdaptiveItemControlSelector()
        {
            this.Loaded += AdaptiveItemControlSelector_Loaded;
            //Content = new UnAdaptedItem();
        }

        private void AdaptiveItemControlSelector_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = DataContext;
            var data = Entity;
            var vm = AdaptiveListViewModel;

            UIElement card;

            switch (data)
            {
                case ImageCarouselCard icc:
                    card = new CarouselCard(icc);
                    break;
                default:
                    card = new UnAdaptedItem(Entity);
                    break;
            }

            Content = card;
        }

        public AdaptiveEntityListViewModel AdaptiveListViewModel { get => (AdaptiveEntityListViewModel)GetValue(AdaptiveListViewModelProperty); set => SetValue(AdaptiveListViewModelProperty, value); }
        public Entity Entity { get => (Entity)GetValue(EntityProperty); set => SetValue(EntityProperty, value); }
    }
}
