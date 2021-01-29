using Coolapk_UWP.Models;
using Coolapk_UWP.Other;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.Controls {
    public sealed partial class DataList : UserControl, INotifyPropertyChanged {
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

        public string Title {
            get { return GetValue(titleProperty)?.ToString(); }
            set { SetValue(titleProperty, value); }
        }

        public string Url {
            get { return GetValue(urlProperty)?.ToString(); }
            set { SetValue(urlProperty, value); }
        }

        public bool TouTiaoMode {
            get { return bool.Parse(GetValue(toutiaoMode)?.ToString() ?? "False"); }
            set { SetValue(toutiaoMode, value); }
        }

        public IncrementalLoadingEntityCollection<Entity> Entities;

        public DataList() {
            this.InitializeComponent();
            //DispatcherPriority.DataBind = 8
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                if (TouTiaoMode) { // 头条模式 也就是首页 URL https://api.coolapk.com/v6/main/indexV8?page=&firstLaunch=&installTime=&lastItem=
                    Set(ref Entities, new IncrementalLoadingEntityCollection<Entity>(async config => {
                        var resp = await App.AppViewModel.CoolapkApis.GetIndexV8(
                            (uint)AppUtil.DateToTimeStamp(DateTime.Now),
                            config.Page,
                            lastItem: config.LastItem
                        );
                        return resp.Data;
                    }), "Entities");
                } else
                    Set(ref Entities, new IncrementalLoadingEntityCollection<Entity>(async config => {
                        var resp = await App.AppViewModel.CoolapkApis.GetDataList(
                            Url,
                            Title,
                            config.Page,
                            lastItem: Entities.Count == 0 ? null : Entities.LastOrDefault(item => item.EntityID > 10000)?.EntityID
                        );
                        return resp.Data;
                    }), "Entities");
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Set<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null) {
            if (Equals(storage, value)) {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void EntityListView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            // f*ck
        }

        private void EntityListView_ItemClick(object sender, ItemClickEventArgs e) {
            var entity = e.ClickedItem;
            if (entity is Feed) {
                var feed = entity as Feed;
                App.AppViewModel.HomeContentFrame.Navigate(typeof(Coolapk_UWP.Pages.FeedDetail), feed.EntityID);
            }
        }
    }
}
