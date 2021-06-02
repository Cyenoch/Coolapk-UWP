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
using Coolapk_UWP.ViewModels;
using Coolapk_UWP.Controls;
using Windows.ApplicationModel.Core;
using Coolapk_UWP.Other;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Coolapk_UWP.Pages
{
    public sealed partial class CommonDataListWrapper : Page
    {
        IncrementalLoadingEntityCollection<Models.Entity> entities;
        ListView listView;

        public CommonDataListWrapper()
        {
            NavigationCacheMode = NavigationCacheMode.Enabled;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Func<LoadMoreItemsAsyncFuncConfig, Task<ICollection<Models.Entity>>> method)
            {
                entities = new IncrementalLoadingEntityCollection<Models.Entity>(method);
                listView = listView ?? new ListView();
                listView.ItemsSource = entities;
                listView.HorizontalAlignment = HorizontalAlignment.Stretch;
                listView.VerticalAlignment = VerticalAlignment.Stretch;

                Content = listView;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            listView?.SetValue(PaddingProperty, new Thickness { Top = sender.Height });
        }
    }

    // 为首页设计的，不周全
    public sealed partial class DataListWrapper : Page
    {
        DataList DataList;
        HomeMenuItem PrePage;

        public DataListWrapper()
        {
            //this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = e.Parameter as HomeMenuItem;
            if (param != null && PrePage != param && e.NavigationMode != NavigationMode.Back)
            {
                PrePage = param;
                DataList = new DataList();
                DataList.SetValue(DataList.titleProperty, param.Config.Title);
                DataList.SetValue(DataList.urlProperty, param.Config.Url);
                DataList.SetValue(DataList.PaddingProperty, new Thickness { Top = CoreApplication.GetCurrentView().TitleBar.Height + 12 });
                if (param.Name == "头条")
                    DataList.SetValue(DataList.toutiaoMode, true);
                Content = DataList;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            DataList.SetValue(DataList.PaddingProperty, new Thickness { Top = sender.Height });
        }
    }
}
