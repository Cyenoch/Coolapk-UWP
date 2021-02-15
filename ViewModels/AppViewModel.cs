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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Coolapk_UWP.ViewModels {
    public class AppViewModel : NotifyPropertyBase {

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        public Frame AppRootFrame { get; set; }
        public Frame HomeContentFrame { get; set; }

        public UserProfile _currentUser;
        public UserProfile CurrentUser {
            get { return _currentUser; }
            set { Set(ref _currentUser, value); }
        }
        public bool IsLogged {
            get { return _currentUser != null; }
        }

        public ICoolapkApis CoolapkApis;
        public AppViewModel() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings() {
                Converters = { new StringEnumConverter() }
            };
            var httpClient = new System.Net.Http.HttpClient(new TokenHeaderHandler()) {
                BaseAddress = new Uri("https://api.coolapk.com"),
            };

            CoolapkApis = RestService.For<ICoolapkApis>(httpClient, new RefitSettings { });

            try {
                LoadLoginState();
            } catch (Exception _) {
                // ignore
            }
        }

        public void Logout() {
            var cookieManager = GetCookieManager();
            var cookies = GetCookies();
            cookies.All((cookie) => {
                cookieManager.DeleteCookie(cookie);
                return true;
            });
            CurrentUser = null;
        }

        public async void LoadLoginState() {
            var uid = GetCookies().FirstOrDefault((cookie) => cookie.Name == "uid")?.Value;
            if (uid == null) return;
            var profile = await CoolapkApis.GetUserProfile((uint)int.Parse(uid.ToString()), AppUtil.DateToTimeStamp(DateTime.Now));
            if (profile.Data == null || profile.Error != null) throw new Exception(profile.Error);
            if (profile.Data.OtherField.ContainsKey("mobilestatus"))
                App.AppViewModel.CurrentUser = profile.Data;
            else throw new Exception("登录失败");
        }

        public IList<HttpCookie> GetCookies() {
            var cookieManager = GetCookieManager();
            var cookieCollection = cookieManager.GetCookies(new Uri("https://coolapk.com/"));
            return cookieCollection.ToList();
        }

        public HttpCookieManager GetCookieManager() {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            return httpBaseProtocolFilter.CookieManager;
        }
    }
}
