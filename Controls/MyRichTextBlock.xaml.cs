using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Coolapk_UWP.Controls {
    /// <summary>
    /// 酷安风格的富文本TextBlock
    /// </summary>
    public sealed partial class MyRichTextBlock : UserControl {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(MyRichTextBlock),
            new PropertyMetadata("", new PropertyChangedCallback(OnTextChanged))
        );
        public static readonly DependencyProperty WrapProperty = DependencyProperty.Register(
            "Wrap",
            typeof(bool),
            typeof(MyRichTextBlock),
            new PropertyMetadata(true)
        );
        public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(
            "IsTextSelectionEnabledProperty",
            typeof(bool),
            typeof(MyRichTextBlock),
            new PropertyMetadata(false)
        );

        /// <summary>
        /// 用于Hyperlink传递href参数
        /// </summary>
        public static readonly DependencyProperty hrefClickParam = DependencyProperty.Register(
            "HrefParam",
            typeof(string),
            typeof(Hyperlink),
            null
        );

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public bool Wrap {
            get { return (bool)GetValue(WrapProperty); }
            set { SetValue(WrapProperty, value); }
        }
        public bool IsTextSelectionEnabled {
            get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
            set { SetValue(IsTextSelectionEnabledProperty, value); }
        }

        public MyRichTextBlock() {
            this.InitializeComponent();
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Load();
            });
        }

        public void Reload(string newValue) {
            Load();
        }

        public void Load() {
            //this.Content = DefaultTemplate.LoadContent() as UIElement;
            RichTextBlock root = new RichTextBlock();
            root.IsTextSelectionEnabled = IsTextSelectionEnabled;
            Paragraph paragraph = new Paragraph();
            //root.TextWrapping = Wrap ? TextWrapping.Wrap : TextWrapping.NoWrap;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            if (!Wrap) {
                doc.LoadHtml(Text.Replace("\n", " "));
            } else {
                doc.LoadHtml(Text);
            }
            var nodes = doc.DocumentNode.ChildNodes;
            foreach (var node in nodes) {
                switch (node.NodeType) {
                    case HtmlAgilityPack.HtmlNodeType.Text:
                        Run run = new Run() { Text = node.InnerText };
                        paragraph.Inlines.Add(run);
                        break;
                    case HtmlAgilityPack.HtmlNodeType.Element:
                        if (node.OriginalName == "a") {
                            // a标签
                            var href = node.GetAttributeValue("href", "");
                            Hyperlink link = new Hyperlink();
                            Run linkRun = new Run {
                                //Text = "{" + node.InnerText + "|" + href + "}"
                                Text = node.InnerText
                            };
                            link.Click += OnHref;
                            link.SetValue(hrefClickParam, href);
                            link.Inlines.Add(linkRun);
                            link.TextDecorations = Windows.UI.Text.TextDecorations.None;
                            Run mf = new Run() { Text = " " };
                            paragraph.Inlines.Add(link);
                            paragraph.Inlines.Add(mf); // 别问 问就是魔法
                        }
                        break;
                }
            }
            root.Blocks.Add(paragraph);
            Content = root;
        }

        public void OnHref(Hyperlink sender, HyperlinkClickEventArgs e) {
            var value = sender.GetValue(hrefClickParam);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            MyRichTextBlock rtb = d as MyRichTextBlock;
            String s = e.NewValue as String;
            rtb.Reload(s);
        }
    }
}
