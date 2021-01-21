using Coolapk_UWP.Models;
using Coolapk_UWP.Other;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Network {
    public partial interface ICoolapkApis {
        [Get("/v6/user/space")]
        Task<Resp<User>> GetUser(uint uid);

        // 已知页面:
        //  用户点评 url	/feed/nodeRatingList?targetType=all&parseRatingToFeed=1&uid={uid}
        //  用户酷图 url	/feed/userCoolPictureFeedList?fragmentTemplate=flex&uid={uid}
        //  用户好物 url	/goods/goodsFeedList?type=default&fragmentTemplate=flex&uid={uid}
        [Get("/v6/page/dataList")]
        Task<CollectionResp<Entity>> GetDataList(string url, string title = "", uint? page = 1, uint? lastItem = default, uint? firstItem = default);

        [Get("/v6/main/indexV8")]
        Task<CollectionResp<Entity>> GetIndexV8(uint installTime, uint? page = 1, uint? firstLaunch = 0, uint? lastItem = default);

        // 获取某人的动态列表
        [Get("/v6/user/feedList")]
        Task<CollectionResp<Entity>> GetUserFeedList(
            uint uid,
            uint page = 1,
            uint lastItem = default,
            uint firstItem = default,
            uint showAnonymouse = 0, // 未知具体作用
            uint isIncludeTop = 1, // 是否包含置顶动态
            uint showDoing = 1
         );

        // 获取某人的图文动态
        [Get("/v6/user/htmlFeedList")]
        Task<CollectionResp<Entity>> GetUserHtmlFeedList(uint uid, uint page = 1, uint lastItem = default);

        // 获取某人的问答动态
        [Get("/v6/user/questionAndAnswerList")]
        Task<CollectionResp<Entity>> GetUserQAList(uint uid, uint page = 1, uint lastItem = default);

        // 获取某人的好物单
        [Get("/v6/goodsList/list")]
        Task<CollectionResp<Entity>> GetUserGoodsList(uint uid, uint page = 1, IList<uint> goodsId = default);

        // 获取用户的收藏夹
        [Get("/v6/collection/list")]
        Task<CollectionResp<Entity>> GetUserCollections(uint uid, uint page = 1, uint showDefault = 0);

        // 获取用户应用集
        [Get("/v6/user/albumlist")]
        Task<CollectionResp<Entity>> GetUserAlbumList(uint uid, uint page = 1);


    }
}
