using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;

namespace Coolapk.WinUI.Utils
{
    public static class CookieHandlerUtil
    {
        public static string Handle(Uri requestUri)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            var cookieManager = httpBaseProtocolFilter.CookieManager;
            var cookieCollection = cookieManager.GetCookies(requestUri);
            var cookieList = cookieCollection.ToList();
            var cookieStr = string.Join(";", cookieList.Select(cookie => $"{cookie.Name}={cookie.Value}"));
            return cookieStr;
        }
    }
}
