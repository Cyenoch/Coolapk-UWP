using Coolapk_UWP.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Network {
    public class ListSortType {
        public const string LastupdateDesc = "lastupdate_desc"; // 默认排序
        public const string DatelineDesc = "dateline_desc"; // 按时间倒序
        public const string Popular = "popular"; // 按
    }

    public class FeedType {
        public const string FeedArticle = "feedArticle";
    }

    public partial interface ICoolapkApis {
        [Get("/v6/feed/detail")]
        Task<Resp<FeedDetail>> GetFeedDetail(uint id);


        [Get("/v6/feed/replyList")]
        Task<CollectionResp<FeedReply>> GetFeedReplyList(
            uint id,
            uint page = 1,
            string listType = ListSortType.LastupdateDesc,
            uint discussMode = 1,
            string feedType = FeedType.FeedArticle,
            uint blockStatus = 0,
            uint fromFeedAuthor = 0,
            uint? lastItem = null,
            uint? firstItem = null);
    }
}
