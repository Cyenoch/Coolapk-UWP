using Coolapk_UWP.Controls;
using Coolapk_UWP.Other;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Coolapk_UWP.Pages
{
    public sealed partial class SearchResult
    {
        public SearchResult()
        {
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            this.InitializeComponent();
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
        }

        private void CoreTitleBar_LayoutMetricsChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args)
        {
            //tabView.SetValue(TabViewHeaderPadding, new Thickness { Top = sender.Height });
            tabView.Resources["TabViewHeaderPadding"] = new Thickness { Top = sender.Height };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string queryText)
            {
                tabView.TabItems.Clear();
                tabView.IsAddTabButtonVisible = false;

                var apis = App.AppViewModel.CoolapkApis;

                var zhDataList = new DataList();
                zhDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, page: config.Page)).Data;


                // https://api.coolapk.com/v6/search?type=product&searchValue=a&page=1&showAnonymous=-1
                // lastItem maybe has a bug
                var digitalDataList = new DataList();
                digitalDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "product", page: config.Page, lastItem: config.LastItem)).Data;


                var userDataList = new DataList();
                userDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "user", page: config.Page, lastItem: config.LastItem)).Data;

                // TODO: sort selector, feed type selector
                var feedDataList = new DataList();
                feedDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "feed", feedType: "all", sort: "default", page: config.Page, lastItem: config.LastItem)).Data;

                var topicDataList = new DataList();
                topicDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "feedTopic", page: config.Page, lastItem: config.LastItem)).Data;

                var goodsDataList = new DataList();
                goodsDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "goods", page: config.Page, lastItem: config.LastItem)).Data;

                // URL	https://api.coolapk.com/v6/search?type=ershou&sort=default&searchValue=a&status=1&deal_type=0&city_code=&is_link=&exchange_type=&ershou_type=&product_id=&page=1
                var ershouDataList = new DataList();
                ershouDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "ershou", page: config.Page, lastItem: config.LastItem)).Data;

                // TODO: feed type selector
                var askDataList = new DataList();
                askDataList.CustomFetchDataEvent += async (config) => (await apis.Search(queryText, type: "ask", feedType: "all", page: config.Page, lastItem: config.LastItem)).Data;

                foreach (var item in new List<TabViewItem>() {
                    new TabViewItem
                    {
                        Header = "动态",
                        Content = feedDataList
                    },
                    new TabViewItem
                    {
                        Header = "综合",
                        Content = zhDataList
                    },
                    new TabViewItem
                    {
                        Header = "数码",
                        Content = digitalDataList
                    },
                    new TabViewItem
                    {
                        Header = "用户",
                        Content = userDataList
                    },
                    new TabViewItem
                    {
                        Header = "话题",
                        Content = topicDataList
                    },
                    new TabViewItem
                    {
                        Header = "好物",
                        Content = goodsDataList
                    },
                    new TabViewItem
                    {
                        Header = "二手",
                        Content = ershouDataList
                    },
                    new TabViewItem
                    {
                        Header = "问答",
                        Content = askDataList
                    }
                })
                {
                    tabView.TabItems.Add(item);
                }


                foreach (var item in tabView.TabItems)
                {
                    ((TabViewItem)item).IsClosable = false;
                }
                ((TabViewItem)tabView.TabItems[0]).IsSelected = true;
            }
        }
    }
}
