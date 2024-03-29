﻿using Coolapk_UWP.Pages;
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

namespace Coolapk_UWP.Controls
{
    /// <summary>
    /// 酷安风格的富文本TextBlock
    /// </summary>
    public sealed partial class MyRichTextBlock : UserControl
    {
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
        public static readonly DependencyProperty LineHeightProperty = DependencyProperty.Register(
            "LineHeight",
            typeof(int),
            typeof(MyRichTextBlock),
            new PropertyMetadata(25, new PropertyChangedCallback(OnTextChanged))
        );

        /// <summary>
        /// 用于Hyperlink传递href参数
        /// </summary>
        public static readonly DependencyProperty HrefClickParam = DependencyProperty.Register(
            "HrefParam",
            typeof(string),
            typeof(Hyperlink),
            null
        );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public bool Wrap
        {
            get { return (bool)GetValue(WrapProperty); }
            set { SetValue(WrapProperty, value); }
        }
        public bool IsTextSelectionEnabled
        {
            get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
            set { SetValue(IsTextSelectionEnabledProperty, value); }
        }
        public int LineHeight
        {
            get { return (int)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        public MyRichTextBlock()
        {
            this.InitializeComponent();
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                LoadContent();
            });
        }

        public void LoadContent()
        {
            // 用来存储子控件
            Paragraph paragraph = new Paragraph();
            paragraph.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            // 去掉换行并加载html字符
            if (!Wrap) doc.LoadHtml(Text.Replace("\n", " "));
            else doc.LoadHtml(Text);
            // 获取所有节点
            var nodes = doc.DocumentNode.ChildNodes;
            foreach (var node in nodes)
            {
                switch (node.NodeType)
                {
                    case HtmlAgilityPack.HtmlNodeType.Text: // 如果是文本节点，创建Run并添加到paragraph
                        Run run = new Run()
                        {
                            Text =
                            node.InnerText
                            .Replace("&gt;", ">")
                            .Replace("&lt;", "<")
                            .Replace("&amp;", "&")
                            .Replace("&apos;", "'")
                            .Replace("&quot;", "\"")
                        };
                        paragraph.Inlines.Add(run);
                        break;
                    case HtmlAgilityPack.HtmlNodeType.Element: // 如果是element
                        if (node.OriginalName == "emoji")
                        {// 如果是表情
                            var src = node.GetAttributeValue("src", "");
                            if (!src.Equals(string.Empty))
                            {
                                BitmapImage bitmapImage = new BitmapImage(new Uri(src));
                                var container = new InlineUIContainer
                                {
                                    Child = new Image
                                    {
                                        Width = paragraph.FontSize + 4,
                                        Height = Width,
                                        Source = bitmapImage,
                                        Margin = new Thickness { Bottom = -4 }
                                    }
                                };
                                paragraph.Inlines.Add(container);
                            }
                        }
                        else if (node.OriginalName == "a")
                        { // 如果是a标签，则是用来跳转的
                          // a标签
                            var href = node.GetAttributeValue("href", "");
                            Hyperlink link = new Hyperlink();
                            Run linkRun = new Run
                            {
                                //Text = "{" + node.InnerText + "|" + href + "}"
                                Text = node.InnerText,
                            };
                            link.Click += OnHref;
                            link.SetValue(HrefClickParam, href); // 使link携带参数，在OnHref中可获取href地址
                            link.Inlines.Add(linkRun);
                            link.TextDecorations = Windows.UI.Text.TextDecorations.None; // 去掉hyperlink下划线
                            paragraph.Inlines.Add(link);
                            Run magic = new Run() { Text = " " };
                            paragraph.Inlines.Add(magic); // 别问 问就是魔法
                        }
                        break;
                }
            }
            RichTextBlock root = new RichTextBlock()
            {
                IsTextSelectionEnabled = IsTextSelectionEnabled,
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight, // 行高
                LineHeight = LineHeight, // 行高
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center
            };
            paragraph.LineHeight = LineHeight;
            root.Blocks.Add(paragraph);
            Content = root;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        public void OnHref(Hyperlink sender, HyperlinkClickEventArgs e)
        {
            var value = sender.GetValue(HrefClickParam);
            switch (value)
            {
                case string strValue:
                    if (strValue.StartsWith("/u/"))
                    {
                        App.AppViewModel.HomeContentFrame.Navigate(typeof(UserProfile), strValue.Replace("/u/", ""));
                    }
                    break;
            }
            // TODO: fuck
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyRichTextBlock rtb = d as MyRichTextBlock;
            rtb.LoadContent();
        }
    }
}
