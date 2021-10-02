using Coolapk_UWP.Models;
using Coolapk_UWP.Network;
using Coolapk_UWP.Other;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;

namespace Coolapk_UWP.ViewModels
{
    public class AppViewModel : NotifyPropertyBase
    {

        public delegate void HomeContentFrameLoadedHandler(Frame HomeContentFrame);
        public event HomeContentFrameLoadedHandler HomeContentFrameLoadedEvent;

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        public Frame AppRootFrame { get; set; }

        private Frame _homeContentFrame;
        public Frame HomeContentFrame
        {
            get { return _homeContentFrame; }
            set
            {
                if (_homeContentFrame == null) HomeContentFrameLoadedEvent?.Invoke(value);
                _homeContentFrame = value;
            }
        }

        private double _appBarHeight;
        public double AppBarHeight
        {
            get => _appBarHeight;
            set => Set(ref _appBarHeight, value);
        }
        public GridLength AppBarHeightGridLength => new GridLength(_appBarHeight);

        public IncrementalLoadingEntityCollection<Entity> NotificationList;

        private CheckNotificationCount checkNotificationCount;
        public CheckNotificationCount CheckNotificationCount
        {
            get => checkNotificationCount;
            set
            {
                Set(ref checkNotificationCount, value);
                UpdateWideTile();
            }
        }

        private UserProfile currentUser;
        public UserProfile CurrentUser
        {
            get => currentUser;
            set
            {
                Set(ref currentUser, value);
                NotificationList = new IncrementalLoadingEntityCollection<Entity>(async (config) =>
                {
                    var resp = (await CoolapkApis.GetNotificationList(config.Page)).Data;
                    var t = new List<Entity>();
                    foreach (var i in resp)
                    {
                        t.Add((Entity)i.AutoCast());
                    }
                    return t;
                });
                UpdateWideTile();
            }
        }
        public bool IsLogged
        {
            get { return currentUser != null; }
        }

        public ICoolapkApis CoolapkApis;
        public AppViewModel()
        {
            App.AppViewModel = this;

            InitRefitService();

            InitLoginState();
        }

        // 更新Wide磁贴内容
        private void UpdateWideTile()
        {
            try
            {
                var notification = NotificationList?.First();
                Newtonsoft.Json.Linq.JToken fromusername = default;
                Newtonsoft.Json.Linq.JToken msg = default;
                if (notification != null)
                {
                    notification.OtherField.TryGetValue("fromusername", out fromusername);
                    notification.OtherField.TryGetValue("note", out msg);
                }
                Tiles.TilesUtil.SetupWideTile(
                   CurrentUser?.Cover ?? "",
                   CurrentUser?.Avatar ?? "",
                   CurrentUser?.Username ?? "",
                   CurrentUser?.Fans ?? 0,
                   unreadNum: CheckNotificationCount?.Badge ?? 0,
                   System.Text.RegularExpressions.Regex.Replace(((string)msg) ?? "", @"：(.+)/a>", ""),
                   (string)fromusername
                );
            }
            catch (Exception _) { }
        }

        // 初始化登录状态
        private async void InitLoginState()
        {
            try
            {
                await LoadLoginState(); // 会重置NotificationList
                await FetchNotificationCount();
                await NotificationList?.LoadMoreItemsAsync(10);
                UpdateWideTile();
            }
            catch (Exception _) { }
        }

        // 登出
        public void Logout()
        {
            var cookieManager = GetCookieManager();
            var cookies = GetCookies();
            cookies.All((cookie) =>
            {
                cookieManager.DeleteCookie(cookie);
                return true;
            });
            CurrentUser = null;
        }

        // 获取未读消息数量
        public async Task FetchNotificationCount()
        {
            try
            {
                var resp = await CoolapkApis.CheckNotificationCount();
                CheckNotificationCount = resp.Data;
            }
            catch (Exception _) { }
        }

        // 载入登录状态
        public async Task LoadLoginState()
        {
            var uid = GetCookies().FirstOrDefault((cookie) => cookie.Name == "uid")?.Value;
            if (uid == null) return;
            var profile = await CoolapkApis.GetUserProfile((uint)int.Parse(uid.ToString()), AppUtil.DateToTimeStamp(DateTime.Now));
            if (profile.Data == null || profile.Error != null) throw new Exception(profile.Error);
            if (profile.Data.OtherField?.ContainsKey("mobilestatus") == true)
                App.AppViewModel.CurrentUser = profile.Data;
            else throw new Exception("登录失败");
            return;
        }

        // 获取cookies
        public IList<HttpCookie> GetCookies()
        {
            var cookieManager = GetCookieManager();
            var cookieCollection = cookieManager.GetCookies(new Uri("https://coolapk.com/"));
            return cookieCollection.ToList();
        }

        // 获取cookiemanager
        public HttpCookieManager GetCookieManager()
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            return httpBaseProtocolFilter.CookieManager;
        }

        private void InitRefitService()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Converters = { new StringEnumConverter() }
            };
            var httpClient = new System.Net.Http.HttpClient(new TokenHeaderHandler(
                    new HttpClientHandler()
                ))
            {
                BaseAddress = new Uri("https://api.coolapk.com"),
            };

            CoolapkApis = RestService.For<ICoolapkApis>(httpClient, new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }),
            });
        }
    }
}
