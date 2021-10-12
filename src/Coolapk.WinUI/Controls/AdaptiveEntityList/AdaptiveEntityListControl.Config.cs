using Coolapk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk.WinUI.Controls.AdaptiveEntityList
{
    public delegate Task<IList<Entity>> AdaptiveEntityListFetcher(AdaptiveEntityListControl control, AdaptiveEntityListViewModel viewModel, AdaptiveEntityListConfig config);
    public class AdaptiveEntityListConfig
    {
        public string Url { get; set; }
        public string Title
        {
            get; set;
        }
        public AdaptiveEntityListFetcher Fetcher { get; set; }
    }
    public static class DefaultFetchers
    {
        public static AdaptiveEntityListFetcher DataListFetcher => async (control, viewModel, config) =>
        {
            var resp = (await viewModel.ApiService.GetDataList(config.Url, config.Title, viewModel.Page, lastItem: viewModel.LastItemID, firstItem: viewModel.FirstItemID));
            if (resp.Error != null) throw new Exception(resp.Error);
            return resp.Data;
        };

        public static AdaptiveEntityListFetcher HomeHeadlineFetcher => async (control, viewModel, config) =>
        {
            var resp = (await viewModel.ApiService.GetIndexV8(0, page: viewModel.Page, lastItem: viewModel.LastItemID));
            if (resp.Error != null) throw new Exception(resp.Error);
            return resp.Data;
        };

    }
}
