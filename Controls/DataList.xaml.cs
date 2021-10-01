using Coolapk_UWP.Models;
using Coolapk_UWP.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.Controls
{
    public sealed partial class DataList : UserControl, INotifyPropertyChanged
    {
        // 对于GetDataList，需要Title参数
        public static readonly DependencyProperty titleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(DataList),
            null
        );
        // 
        public static readonly DependencyProperty urlProperty = DependencyProperty.Register(
            "Url",
            typeof(string),
            typeof(DataList),
            null
        );
        // 如果是头条页，则用另一个请求接口 复用该组件
        public static readonly DependencyProperty toutiaoMode = DependencyProperty.Register(
            "TouTiao",
            typeof(bool),
            typeof(DataList),
            new PropertyMetadata(false)
        );
        public static readonly DependencyProperty customMode = DependencyProperty.Register(
             "CustomMode",
             typeof(bool),
             typeof(DataList),
             new PropertyMetadata(false)
         );

        public event Func<LoadMoreItemsAsyncFuncConfig, Task<ICollection<Models.Entity>>> CustomFetchDataEvent;

        public string Title
        {
            get { return GetValue(titleProperty)?.ToString(); }
            set { SetValue(titleProperty, value); }
        }

        public string Url
        {
            get { return GetValue(urlProperty)?.ToString(); }
            set { SetValue(urlProperty, value); }
        }

        public bool TouTiaoMode
        {
            get { return bool.Parse(GetValue(toutiaoMode)?.ToString() ?? "False"); }
            set { SetValue(toutiaoMode, value); }
        }

        public IncrementalLoadingEntityCollection<Entity> Entities;

        public uint installTime = (uint)AppUtil.DateToTimeStamp(DateTime.Now);

        public DataList()
        {
            this.InitializeComponent();
            //DispatcherPriority.DataBind = 8
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (CustomFetchDataEvent != null)
                {
                    Set(ref Entities, new IncrementalLoadingEntityCollection<Entity>(CustomFetchDataEvent), "Entities");
                }
                else if (TouTiaoMode)
                { // 头条模式 也就是首页 URL https://api.coolapk.com/v6/main/indexV8?page=&firstLaunch=&installTime=&lastItem=
                    Set(ref Entities, new IncrementalLoadingEntityCollection<Entity>(async config =>
                    {
                        var resp = await App.AppViewModel.CoolapkApis.GetIndexV8(
                            installTime,
                            config.Page,
                            lastItem: config.LastItem
                        );
                        return resp.Data;
                    }), "Entities");
                }
                else
                    Set(ref Entities, new IncrementalLoadingEntityCollection<Entity>(async config =>
                    {
                        var resp = await App.AppViewModel.CoolapkApis.GetDataList(
                            Url,
                            Title,
                            config.Page,
                            lastItem: Entities.Count == 0 ? null : Entities.LastOrDefault(item => item.EntityID > 10000)?.EntityID
                        );
                        return resp.Data;
                    }), "Entities");
                //_ = Entities.LoadMoreItemsAsync(20);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Set<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void EntityListView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // f*ck
        }

        private void EntityListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var entity = e.ClickedItem;
            if (entity is Feed)
            {
                // feed类留给相应item自行处理
                //var feed = entity as Feed;
                //App.AppViewModel.HomeContentFrame.Navigate(typeof(Coolapk_UWP.Pages.FeedDetail), feed.EntityID);
            }
            if (entity is RefreshCard)
            {
                Entities.Reload();
            }
        }

        private void RefreshContainer_RefreshRequested(Microsoft.UI.Xaml.Controls.RefreshContainer sender, Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs args)
        {
            var work = Entities.Reload();
            work.ContinueWith(action => { 
                if (action.Status == TaskStatus.RanToCompletion)
                {
                    _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                        args.GetDeferral().Complete();
                    });
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Entities.Reload();
        }
    }
}
