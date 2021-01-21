using Coolapk_UWP.Network;
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

namespace Coolapk_UWP.ViewModels {
    public class AppViewModel {
        public ICoolapkApis CoolapkApis;
        public AppViewModel() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings() {
                Converters = { new StringEnumConverter() }
            };

            CoolapkApis = RestService.For<ICoolapkApis>(new HttpClient(new TokenHeaderHandler()) {
                BaseAddress = new Uri("https://api.coolapk.com")
            }, new RefitSettings {
            });
        }
    }
}
