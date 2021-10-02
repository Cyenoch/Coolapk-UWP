using Coolapk.Common;
using Coolapk_UWP.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;

namespace Coolapk_UWP.Network {
    public class TokenHeaderHandler : DelegatingHandler {
        private static readonly string guid = Guid.NewGuid().ToString();
        public string Token {
            get {
                var t = Utils.DateTimeToUnixTimeStamp(DateTime.Now);
                var hexT = "0x" + Convert.ToString((int)t, 16);
                var md5T = Utils.GetMD5($"{t}");
                var a = $"token://com.coolapk.market/c67ef5943784d09750dcfbb31020f0ab?{md5T}${guid}&com.coolapk.market";
                var md5A = Utils.GetMD5(Convert.ToBase64String(Encoding.UTF8.GetBytes(a)));
                var token = md5A + guid + hexT;
                return token;
            }
        }
        public TokenHeaderHandler(HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler()) {

        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {

            request.Headers.Add("x-app-token", Token);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("X-Sdk-Int", "28");
            request.Headers.Add("X-Sdk-Locale", "zh-CN");
            request.Headers.Add("X-App-Id", "com.coolapk.market");
            request.Headers.Add("X-App-Version", "11.0");
            request.Headers.Add("X-App-Code", "2101202");
            request.Headers.Add("X-Api-Version", "11");
            request.Headers.Add("X-App-Device", Utils.GetMD5(guid));

            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            var cookieManager = httpBaseProtocolFilter.CookieManager;
            var cookieCollection = cookieManager.GetCookies(request.RequestUri);
            var x = cookieCollection.ToList();
            request.Headers.Add("cookie", string.Join(";", x.Select(cookie => $"{cookie.Name}={cookie.Value}")));

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

    }
}
