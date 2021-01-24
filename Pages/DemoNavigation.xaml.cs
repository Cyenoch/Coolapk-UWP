using Coolapk_UWP.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace Coolapk_UWP.Pages {
    public class DemoNavItem {
        public string Name { get; set; }
        public Type PageType { get; set; }
        public object Param { get; set; }
    }

    public sealed partial class DemoNavigation : Page {
        public ObservableCollection<DemoNavItem> NavItems = new ObservableCollection<DemoNavItem>()
        {
            new DemoNavItem
            {
                Name = "用户信息页Demo",
                PageType = typeof(UserProfile),
            },


            new DemoNavItem
            {
                Name = "IndexV8 Demo",
                PageType = typeof(DemoIndexV8),
            },

            new DemoNavItem
            {
                Name = "Feed List Demo",
                PageType = typeof(DemoFeedList),
            },

            new DemoNavItem
            {
                Name = "Feed Detail Demo (HtmlArticle)",
                PageType = typeof(FeedDetail),
            }
        };

        public DemoNavigation() {
            this.InitializeComponent();
        }

        private void DemoNavItemList_OnItemClick(object sender, ItemClickEventArgs e) {
            try {
                ListView listView = (ListView)sender;
                DemoNavItem clickedItem = (DemoNavItem)e.ClickedItem;
                (Window.Current.Content as Frame).Navigate(clickedItem.PageType);
            } catch (Exception err) {
                Debug.Print(err.StackTrace);
            }
        }
    }
}