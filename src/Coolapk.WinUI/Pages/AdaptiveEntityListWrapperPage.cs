using Coolapk.WinUI.Controls.AdaptiveEntityList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Coolapk.WinUI.Pages
{
    public class AdaptiveEntityListWrapperPage : Page
    {
        public AdaptiveEntityListWrapperPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        IDictionary<string, AdaptiveEntityListControl> cache = new Dictionary<string, AdaptiveEntityListControl>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            switch (e.Parameter)
            {
                case AdaptiveEntityListConfig config:
                    if (cache.ContainsKey(config.Title))
                    {
                        Content = cache[config.Title];
                    }
                    else
                    {
                        Content = new AdaptiveEntityListControl(config);
                        cache[config.Title] = (AdaptiveEntityListControl)Content;
                    }
                    break;
            }
        }
    }
}
