using Coolapk.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk.WinUI.TemplateSelectors
{
    public sealed class HomeNavigationViewMenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DividerMenuItemTemplate { get; set; }
        public DataTemplate MultiChildMenuItemTemplate { get; set; }
        public DataTemplate ImageMenuItemTemplate { get; set; }
        public DataTemplate SymbolIconMenuItemTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case DividerItem _:
                    return DividerMenuItemTemplate;
                case SelectableItem selectableItem:
                    return selectableItem.FallbackLogo == default ? ImageMenuItemTemplate : SymbolIconMenuItemTemplate;
                case MultiChildItem _:
                    return MultiChildMenuItemTemplate;
                default:
                    throw new Exception("no navigation menu item template found");
            }
        }
    }
}
