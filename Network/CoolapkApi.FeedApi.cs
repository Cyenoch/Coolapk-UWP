using Coolapk_UWP.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Network {
    public partial interface ICoolapkApis {
        [Get("/v6/feed/detail")]
        Task<Resp<FeedDetail>> GetFeedDetail(uint id);
    }
}
