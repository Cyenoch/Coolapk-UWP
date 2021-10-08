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
        public static AdaptiveEntityListFetcher DataListFetcher => (control, viewModel, config) =>
        {
            return null;
        };
    }
}
