using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Coolapk.Api;
using Coolapk.Apis;
using Coolapk.WinUI.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using Refit;
using Splat;

namespace Coolapk.WinUI
{
    public partial class App
    {
        private void Setup()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            Locator.CurrentMutable.RegisterConstant(SetupRefitService(), typeof(ICoolapkApis));
        }

        private ICoolapkApis SetupRefitService()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Converters = { new StringEnumConverter() }
            };
            var httpClient = new HttpClient(new TokenHeaderHandler(CookieHandlerUtil.Handle))
            {
                BaseAddress = new Uri("https://api.coolapk.com"),
            };
            return RestService.For<ICoolapkApis>(httpClient, new RefitSettings
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
