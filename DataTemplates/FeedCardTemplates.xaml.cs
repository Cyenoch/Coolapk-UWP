using Coolapk_UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.DataTemplates {

    public class FeedCardTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            Feed feed = (Feed)item;
            return DefaultTemplate;
        }
    }

    public partial class FeedCardTemplates : ResourceDictionary {
        public FeedCardTemplates() {
            this.InitializeComponent();
        }
    }
}
