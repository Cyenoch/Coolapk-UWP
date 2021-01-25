using Coolapk_UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.ViewModels {
    public class FeedDetailViewModel : AsyncLoadViewModel<FeedDetail> {
        public uint FeedId = 18484842;

        public FeedDetailViewModel() {
            this.Reload();
        }

        public override async Task<RespBase<FeedDetail>> OnLoadAsync() {
            var resp = await CoolapkApis.GetFeedDetail(FeedId);
            return resp;
        }

        public override string[] NotifyChangedProperties() {
            return base.NotifyChangedProperties();
        }
    }
}
