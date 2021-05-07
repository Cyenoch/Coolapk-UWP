using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Coolapk_UWP.Models;
using Coolapk_UWP.Network;
using Windows.Storage;
using Aliyun.OSS.Util;
using Aliyun.OSS;
using Newtonsoft.Json;
using Aliyun.OSS.Common;
using Windows.UI.Core;

namespace Coolapk_UWP.Controls {
    public sealed partial class PicTextEditor : UserControl {
        public MessageNodeList NodeList;

        // md5 : http addr
        public Dictionary<string, string> UploadedImg = new Dictionary<string, string> { }; // 已上传的图片做缓存

        private StackPanel panel;
        public PicTextEditor() {
            NodeList = new MessageNodeList(ReLayout);
            NodeList.Add(new TextNode(NodeList, "酷安 UWP 图文编辑器"));
        }
        /// <summary>
        /// 上传图片和生成结构体
        /// </summary>
        /// <returns></returns>
        public async Task<IList<MessageRawStructBase>> UploadAndGenerateStructModel() {
            var list = new List<MessageRawStructBase>();
            // 寻找还未上传的图片node
            var waitForUpload = NodeList.Where(node => node is ImageNode).Where(node => !UploadedImg.ContainsKey((node as ImageNode).HashMd5)).Select(node => node as ImageNode).ToList();
            // 生成上传所需的信息
            var fragments = new List<UploadFileFragment>();

            foreach (var node in waitForUpload) {
                if (fragments.Any(_n => _n.Md5.Equals(node.HashMd5))) break;
                var img = await BitmapDecoder.CreateAsync(await node.File.OpenAsync(FileAccessMode.Read));
                fragments.Add(new UploadFileFragment {
                    Resolution = $"{img.PixelWidth}x{img.PixelHeight}",
                    Name = node.HashMd5 + node.File.FileType,
                    Md5 = node.HashMd5
                });
            }

            if (fragments.Count > 0) {

                var dialog = new ContentDialog() {
                    Title = "正在上传..."
                };
                _ = dialog.ShowAsync();
                try {

                    // 从酷安获取STS
                    var r = await App.AppViewModel.CoolapkApis.OssUploadPicturePrepare(new OssUploadPicturePrepareBody {
                        UploadFileFragmentsSource = fragments
                    });
                    var data = r.Data;
                    // 对每个需要上传的文件执行真正的上传操作

                    dialog.Content = "正在生成上传任务...";



                    // 生成tasks
                    var tasks = data.FileInfo.Select(async info => {
                        var imgNode = waitForUpload.Find(node => node.HashMd5 == info.Md5);
                        using (var fs = await imgNode.File.OpenStreamForReadAsync()) {
                            var response = CoolapkApiHelper.OssUpload(data.UploadPrepareInfo, info, fs, imgNode.File.ContentType,
                                (OssUploadPicturePrepareResultFileInfo fileInfo, object sender, StreamTransferProgressArgs args) => {
                                    _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                                        dialog.Content = $"{data.FileInfo.IndexOf(info) + 1}/{data.FileInfo.Count} 上传进度 {args.PercentDone}%";
                                    });
                                });
                            // 添加到已上传的图片的列表防止重复上传
                            UploadedImg.Add(imgNode.HashMd5, response.Data.Url);
                            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                                dialog.Content = $"{data.FileInfo.IndexOf(info) + 1}/{data.FileInfo.Count} 上传完成!";
                            });
                        }
                    }).ToArray();

                    // 逐个执行
                    await Task.WhenAll(tasks);
                    _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        dialog.Title = "上传完成";
                        dialog.Content = "所有上传任务完成";
                        dialog.Hide();
                    });

                    //
                } catch (OssException err) {
                    //throw new Exception($"某张图片上传失败: {err.HResult}:{err.ErrorCode} -> {err.Message}");
                    dialog.Title = "上传失败";
                    dialog.Content = ($"某张图片上传失败: {err.HResult}:{err.ErrorCode} -> {err.Message}");
                } catch (Exception err) {
                    //throw new Exception("上传图片上传失败: " + err.Message);
                    dialog.Title = "上传失败";
                    dialog.Content = ("上传图片上传失败: " + err.Message);
                } finally {
                    dialog.PrimaryButtonText = "关闭";
                }
            }

            // 遍历NodeList生成RawStruct
            foreach (var node in NodeList) {
                switch (node) {
                    case TextNode textNode:
                        list.Add(new MessageRawStructText { Type = "text", Message = textNode.Text });
                        break;
                    case ImageNode imageNode:
                        // 从已上传的图片中获取链接
                        string uploadedUrl = UploadedImg[imageNode.HashMd5];
                        list.Add(new MessageRawStructImage { Type = "image", Description = imageNode.IntroText, Url = uploadedUrl });
                        break;
                }

            }
            return list;
        }

        public void ReLayout() {
            if (panel != null) {
                panel.Children.Clear();
                foreach (var node in NodeList) {
                    if (node.Element != null) panel.Children.Add(node.Element);
                }
            } else {
                // 首次
                panel = new StackPanel() {
                    Margin = new Thickness { Top = 12, Bottom = 100 },
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                foreach (var node in NodeList) {
                    if (node.Element != null) panel.Children.Add(node.Element);
                }
                Content = panel;
            }
        }

        public abstract class NodeBase {
            public abstract UIElement Element { get; }
            public void InsertAfter(NodeBase node) =>
                RootList.Insert(CurrentIndex + 1, node);
            public void InsertBefore(NodeBase node) =>
                RootList.Insert(CurrentIndex, node);

            public virtual bool CanMerge(NodeBase node) => false;
            public virtual void Merge(NodeBase node) { }
            public virtual bool CanSplitInsert(NodeBase node) => false; // 标记是否可以分割插入
            public virtual void SplitInsert(NodeBase node) { }
            public MessageNodeList RootList { get; set; }
            public int CurrentIndex => RootList.IndexOf(this);
            public bool IsFirst => RootList.IndexOf(this) == 0;
            public bool IsLast => RootList.IndexOf(this) == RootList.Count - 1;
            public NodeBase PreNode => RootList.Count > 0 && CurrentIndex > 0 ? RootList[CurrentIndex - 1] : null;
            public NodeBase NextNode => RootList.Count > 0 && CurrentIndex < RootList.Count - 2 ? RootList[CurrentIndex + 1] : null;
            public NodeBase(MessageNodeList RootList) => this.RootList = RootList;
            public virtual void LostFocus(object sender, RoutedEventArgs e) {
                if (!RootList.FlagLosingFocus) RootList.FocusIndex = -1;
            }
            public virtual void GotFocus(object sender, RoutedEventArgs e) => RootList.FocusIndex = CurrentIndex;

        }
        public class MessageNodeList : Collection<NodeBase> {
            public delegate void ReLayoutRequestDelegate();
            public ReLayoutRequestDelegate ReLayoutRequest;
            public MessageNodeList(ReLayoutRequestDelegate reLayoutRequest) { this.ReLayoutRequest = reLayoutRequest; }
            public bool FlagLosingFocus = false; // 文件选择导致focus丢失flag
            public int FocusIndex { get; set; } = -1;
            public void AutoInsert(NodeBase node) {
                if (FocusIndex == -1) {
                    InsertItem(Count, node);
                } else {
                    if (this[FocusIndex].CanSplitInsert(node)) {
                        this[FocusIndex].SplitInsert(node);
                    } else
                        InsertItem(FocusIndex + 1, node);
                }
            }
            protected override void RemoveItem(int index) {
                var node = this[index];
                var pre = node.PreNode;
                var next = (index + 1) == Count ? null : this[index + 1];
                if (pre != null && next != null) {
                    if (pre.CanMerge(next)) {
                        pre.Merge(next);
                        base.RemoveItem(next.CurrentIndex);
                    }
                }
                base.RemoveItem(index);
                ReLayoutRequest();
            }
            protected override void InsertItem(int index, NodeBase item) {
                if (Count == 0) base.InsertItem(index, item);
                else if (Count > 0) {
                    if (index == 0) { // 在行首插入
                        // 检查下一个
                        var nextNode = Count > 1 ? this[1] : null;
                        if (nextNode != null && item.CanMerge(nextNode))
                            item.Merge(nextNode);
                        else base.InsertItem(0, item);

                        if (item is ImageNode && nextNode is ImageNode) {
                            // 之间需要插入一条文本
                            InsertItem(index, new TextNode(this, ""));
                        }
                        if (item is ImageNode) {
                            base.InsertItem(0, new TextNode(this, ""));
                        }
                    } else if (index > 0 && index < Count) { // 在中间插入
                        var nextNode = this[index];
                        var preNode = this[index - 1];
                        if (nextNode != null && item.CanMerge(nextNode)) {
                            item.Merge(nextNode);
                            base.InsertItem(index, item);
                            Remove(nextNode);
                        } else if (preNode != null && preNode.CanMerge(item)) {
                            preNode.Merge(item);
                        } else base.InsertItem(index, item);

                        if (item is ImageNode && nextNode is ImageNode) {
                            InsertItem(index + 1, new TextNode(this, ""));
                        }
                        if (item is ImageNode && preNode is ImageNode) {
                            InsertItem(index, new TextNode(this, ""));
                        }
                    } else if (index == Count) { // 在末尾追加
                        // 检查上一个
                        var preNode = this[index - 1];
                        if (preNode != null && preNode.CanMerge(item))
                            preNode.Merge(item);
                        else base.InsertItem(Count, item);

                        if (item is ImageNode && preNode is ImageNode) {
                            // 图片与图片之间需要插入一段文本
                            InsertItem(index, new TextNode(this, ""));
                        }
                        if (item is ImageNode) {
                            InsertItem(index + 1, new TextNode(this, ""));
                        }
                    }
                }
                ReLayoutRequest?.Invoke();
            }
        }
        public class TextNode : NodeBase {
            private int selectionIndex; // 失去焦点时，光标的字符位置
            public TextNode(MessageNodeList NodeList, string initText) : base(NodeList) {
                EditBox = new TextBox {
                    Text = initText,
                    TextWrapping = TextWrapping.Wrap,
                    AcceptsReturn = true,
                    Width = 600,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderThickness = new Thickness { Top = 2, Left = 2, Right = 2, Bottom = 2 },
                    BorderBrush = App.Current.Resources["SystemControlAltLowAcrylicElementBrush"] as Brush
                };
                EditBox.GotFocus += GotFocus;
                EditBox.LostFocus += LostFocus;
            }
            public override bool CanMerge(NodeBase node) => node is TextNode;
            public override void Merge(NodeBase node) {
                // 不考虑光标位置，追加到末尾
                switch (node) {
                    case TextNode textNode:
                        EditBox.Text += textNode.EditBox.Text;
                        break;
                }
            }
            public TextBox EditBox;
            public string Text => EditBox.Text;
            public override UIElement Element { get => EditBox; }
            public override bool CanSplitInsert(NodeBase node) => node is ImageNode;
            public override void SplitInsert(NodeBase node) {
                if (selectionIndex == 0) {
                    InsertBefore(node);
                } else if (selectionIndex == EditBox.Text.Length) {
                    InsertAfter(node);
                } else {
                    var firstHalf = EditBox.Text.Substring(0, selectionIndex);
                    var secondHalf = EditBox.Text.Substring(selectionIndex);
                    InsertAfter(node);
                    var nnode = new TextNode(RootList, secondHalf); // 遇到\r起手的不知道为什么会变成空的
                    nnode.EditBox.Text = secondHalf.Replace("\r", "");
                    RootList.Insert(CurrentIndex + 2, nnode);
                    EditBox.Text = firstHalf;
                }
            }
            public override void GotFocus(object sender, RoutedEventArgs e) {
                base.GotFocus(sender, e);
            }
            public override void LostFocus(object sender, RoutedEventArgs e) {
                selectionIndex = EditBox.SelectionStart;
                base.LostFocus(sender, e);
            }
        }

        public class ImageNode : NodeBase {
            public string HashMd5;
            public BitmapImage ImageSource => Image.Source as BitmapImage;
            public string IntroText => Intro.Text;
            public StorageFile File;
            private async void Init() {
                using (var fileStream = await File.OpenAsync(FileAccessMode.Read)) {
                    var img = new BitmapImage();
                    await img.SetSourceAsync(fileStream);
                    Image.Source = img;
                }
            }
            public ImageNode(MessageNodeList NodeList, StorageFile file, string hashMd5) : base(NodeList) {
                File = file;
                Init();
                HashMd5 = hashMd5;
                var cbtn = new Button {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = new SymbolIcon {
                        Symbol = Symbol.Cancel
                    }
                };

                cbtn.Click += Cbtn_Click;
                var stack = new StackPanel {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                stack.Children.Add(cbtn);
                stack.Children.Add(Image);
                Panel.Children.Add(stack);
                Panel.Children.Add(Intro);
                Intro.GotFocus += GotFocus;
                Intro.LostFocus += LostFocus;
            }

            private void Cbtn_Click(object sender, RoutedEventArgs e) {
                RootList.Remove(this);
            }

            public StackPanel Panel = new StackPanel {
                Orientation = Orientation.Vertical,
                Padding = new Thickness { Top = 6, Bottom = 6, Left = 24, Right = 24 }
            };
            private readonly ImageEx Image = new ImageEx() {
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center,
                CornerRadius = new CornerRadius(8),
                MaxWidth = 600
            };
            private readonly TextBox Intro = new TextBox() {
                HorizontalAlignment = HorizontalAlignment.Center,
                PlaceholderText = "图片介绍文本",
                BorderThickness = new Thickness(),
                TextAlignment = TextAlignment.Center,
                MaxWidth = 500,
            };
            public override UIElement Element { get => Panel; }
        }

    }
}
