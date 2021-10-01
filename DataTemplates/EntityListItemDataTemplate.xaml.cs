using Coolapk_UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk_UWP.DataTemplates {
    /// <summary>
    /// 注册模板需要做件事情
    /// 1: 在Models文件夹下创建继承Entity的类 比如 public class Example:Entity {}
    /// 2: 在EntityListItemTemplateSelector类中创建一个 DataTemplate 模板名 {get;set;} 字段， 比如 public DataTemplate ExampleTemplate {get;set;}
    /// 3: 在EntityListItemDatatempalte.xaml中创建一个DataTemplate x:Key自己设，比如<DataTemplate x:Key="ExampleCardTemplate">模板不要是空的</DataTemplate
    /// 4: 在EntityListItemDatatempalte.xaml中的templates:EntityListItemTemplateSelector 下注册你的模板，如: ExampleTemplate(这个是第二部创建的字段的名称)="{StaticResource ExampleCardTemplate}"
    /// 5: 在EntityListItemTemplateSelector类下的SelectTemplateCore方法中的switch块中添加一个case Example value: return ExampleTemplate(这个是第二部创建的字段的名字);
    /// </summary>
    public class EntityListItemTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CarouselCardTemplate { get; set; } // 轮播
        public DataTemplate IconScrollCardTemplate { get; set; } // 
        public DataTemplate TitleCardTemplate { get; set; } // 
        public FeedCardTemplateSelector FeedCardTemplateSelector { get; set; }
        public DataTemplate FeedReplyTemplate { get; set; }
        public DataTemplate IconLinkGridCardTemplate { get; set; }
        public DataTemplate ImageTextScrollCardTemplate { get; set; }
        public DataTemplate RefreshCardTemplate { get; set; }
        // 根据item的对象类型分配模板
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject c) {
            switch (item) {
                case ImageCarouselCard _:
                    return CarouselCardTemplate;
                case IconScrollCard _:
                    return IconScrollCardTemplate;
                case TitleCard _:
                    return TitleCardTemplate;
                case Feed _:
                    return FeedCardTemplateSelector.SelectTemplate(item, c);
                case FeedReply _:
                    return FeedReplyTemplate;
                case IconLinkGridCard _:
                    return IconLinkGridCardTemplate;
                case ImageTextScrollCard _:
                    return ImageTextScrollCardTemplate;
                case RefreshCard _:
                    return RefreshCardTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }

    public class DataListItemContainerStyleSelector : StyleSelector {
        public Style WidthLimitedContainer { get; set; }
        public Style WidthNoLimitedContainer { get; set; }
        public Style WidthLimitedFeedContainerStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container) {
            switch (item) {
                case IconScrollCard _:
                case IconLinkGridCard _:
                case ImageCarouselCard _:
                    return WidthLimitedContainer;
                case Feed _:
                    return WidthLimitedFeedContainerStyle;
            }
            return WidthLimitedContainer;
        }
    }


    public partial class EntityListItemDataTemplate : ResourceDictionary {
        public EntityListItemDataTemplate() {
            this.InitializeComponent();
        }

        private void GoToMore_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
