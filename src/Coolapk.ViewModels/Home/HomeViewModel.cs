using Coolapk.Apis;
using Coolapk.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Coolapk.ViewModels.Home
{
    public abstract class MenuItem
    {
        public string Title { get; set; }
    }
    public class SelectableItem : MenuItem
    {
        public Symbol FallbackLogo => Logo.Length > 0 ? default : Symbol.Placeholder;
        public string Logo { get; set; }
        public MainInitTabConfig Config { get; set; }
    }
    public class MultiChildItem : MenuItem
    {
        public Symbol Icon { get; set; }
        public IEnumerable<MenuItem> SubItem { get; set; }
        public int DefaultSelect { get; set; }
    }
    public class DividerItem : MenuItem { }

    public partial class HomeViewModel : ApiRequestViewModelBase, IApiRequestViewModel
    {
        [Reactive]
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public HomeViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>();
        }

        public async Task InitializeRequestAsync()
        {
            try
            {
                var config = await ApiService.GetMainInit();

                var homeTabs = config.Data[1].Tabs;
                var digitalTabs = config.Data[3].Tabs;
                var discoveryTabs = config.Data[2].Tabs;

                homeTabs.Select(tab =>
                {
                    return new SelectableItem()
                    {
                        Title = tab.Title,
                        Config = tab,
                        Logo = tab.Logo,
                    };
                }).ToList().ForEach(MenuItems.Add);

                MenuItems.Add(new DividerItem());

                MenuItems.Add(new MultiChildItem()
                {
                    Title = "数码",
                    Icon = Symbol.CellPhone,
                    SubItem = digitalTabs.Select(tab =>
                    {
                        return new SelectableItem()
                        {
                            Title = tab.Title,
                            Config = tab,
                            Logo = tab.Logo,
                        };
                    }),
                });
                MenuItems.Add(new MultiChildItem()
                {
                    Title = "发现",
                    Icon = Symbol.Find,
                    SubItem = discoveryTabs.Select(tab =>
                    {
                        return new SelectableItem()
                        {
                            Title = tab.Title,
                            Config = tab,
                            Logo = tab.Logo,
                        };
                    }),
                });
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
