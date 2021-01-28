using Coolapk_UWP.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Network {
    public partial interface ICoolapkApis {
        // 已知页面:
        //  用户点评 url	/feed/nodeRatingList?targetType=all&parseRatingToFeed=1&uid={uid}
        //  用户酷图 url	/feed/userCoolPictureFeedList?fragmentTemplate=flex&uid={uid}
        //  用户好物 url	/goods/goodsFeedList?type=default&fragmentTemplate=flex&uid={uid}
        [Get("/v6/page/dataList")]
        Task<CollectionResp<Entity>> GetDataList(string url, string title = "", uint? page = 1, uint? lastItem = default, uint? firstItem = null);

        [Get("/v6/main/indexV8")]
        Task<CollectionResp<Entity>> GetIndexV8(uint installTime, uint? page = 1, uint? firstLaunch = 0, uint? lastItem = null);

        [Get("/v6/main/init")]
        Task<CollectionResp<MainInit>> GetMainInit();
    }
}
