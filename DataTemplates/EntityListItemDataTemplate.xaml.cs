using Coolapk_UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.DataTemplates {
    public class EntityListItemTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CarouselCardTemplate { get; set; }
        // 根据item的对象类型分配模板
        protected override DataTemplate SelectTemplateCore(object item) {
            switch (item) {
                case ImageCarouselCard _:
                    return CarouselCardTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }

    public partial class EntityListItemDataTemplate : ResourceDictionary {
        public EntityListItemDataTemplate() {
            this.InitializeComponent();
        }
    }
}
