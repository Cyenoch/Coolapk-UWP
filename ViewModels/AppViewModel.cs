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

namespace Coolapk_UWP.ViewModels
{
    public class AppViewModel : NotifyPropertyBase
    {

        public delegate void HomeContentFrameLoadedHandler(Frame HomeContentFrame);
        public event HomeContentFrameLoadedHandler HomeContentFrameLoadedEvent;

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        public Frame AppRootFrame { get; set; }

        public Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.LeftCompact;
        public event PaneDisplayModeChangedHandle PaneDisplayModeChanged;
        public delegate void PaneDisplayModeChangedHandle(Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode mode);

        public void FirePaneDisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode mode)
        {
            App.AppViewModel.PaneDisplayModeChanged?.Invoke(mode);
        }

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

        private UserProfile _currentUser;
        public UserProfile CurrentUser
        {
            get { return _currentUser; }
            set { 
                Tiles.TilesUtil.SetupWideTile(value.Avatar, value.Username, value.Fans); 
                Set(ref _currentUser, value);
            }
        }
        public bool IsLogged
        {
            get { return _currentUser != null; }
        }

        public ICoolapkApis CoolapkApis;
        public AppViewModel()
        {

            App.AppViewModel = this;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Converters = { new StringEnumConverter() }
            };
            var httpClient = new System.Net.Http.HttpClient(new TokenHeaderHandler(
                    new HttpClientHandler
                    {
                    }
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

            InitLoginState();
        }

        private async void InitLoginState()
        {
            try
            {
                await LoadLoginState();
            }
            catch (Exception)
            {
                // ignore
            }
        }

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

        public IList<HttpCookie> GetCookies()
        {
            var cookieManager = GetCookieManager();
            var cookieCollection = cookieManager.GetCookies(new Uri("https://coolapk.com/"));
            return cookieCollection.ToList();
        }

        public HttpCookieManager GetCookieManager()
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            return httpBaseProtocolFilter.CookieManager;
        }
    }
}
