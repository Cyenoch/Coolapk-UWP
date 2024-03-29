# Coolapk-UWP

**这是一个兴趣使然的项目，主要目的用于学习 UWP 应用开发，以及 C#语言**

其次，为了方便在 Windows 上刷酷安

---

目前支持的设备架构: **arm, x86, x64**

是的，你可以在**windows arm设备**上运行本应用! (但还没试过)

> **仅供个人用于学习、研究;不得用于商业用途;**

### 预览效果 （Mica!）

[![4Ttux1.md.png](https://z3.ax1x.com/2021/10/01/4Ttux1.md.png)](https://imgtu.com/i/4Ttux1)
[![4TtmG9.md.png](https://z3.ax1x.com/2021/10/01/4TtmG9.md.png)](https://imgtu.com/i/4TtmG9)
[![4TtlqK.md.png](https://z3.ax1x.com/2021/10/01/4TtlqK.md.png)](https://imgtu.com/i/4TtlqK)
[![4TtePJ.md.png](https://z3.ax1x.com/2021/10/01/4TtePJ.md.png)](https://imgtu.com/i/4TtePJ)
[![4Ttn2R.md.png](https://z3.ax1x.com/2021/10/01/4Ttn2R.md.png)](https://imgtu.com/i/4Ttn2R)
[![4TtMKx.md.png](https://z3.ax1x.com/2021/10/01/4TtMKx.md.png)](https://imgtu.com/i/4TtMKx)
[![4TtQr6.md.png](https://z3.ax1x.com/2021/10/01/4TtQr6.md.png)](https://imgtu.com/i/4TtQr6)

### 适配酷安的 URI SCHEME

- 打开动态
  > coolmarket://feed/ + feed id

### 简要说明各目录及文件作用:

- Assets 资源文件
- Controls 控件
  - AsyncLoadStateControl.xaml 针对一波获取的页面提供三种状态
  - DataList.xaml 数据列表
  - MyRichTextBlock.xaml 富文本编辑器实现
  - PicArrBox.xaml 宫格图片
  - ReplyList.xaml 回复列表
- DataTemplates 模板
  - EntityListItemDataTemplate.xaml 提供一个 TemplateSelector 和简单的模板用于动态列表
  - FeedCardTemplates.xaml 和动态有关的 Item 的模板
  - FeedReplyTemplate.xaml 和评论有关的 Item 模板以及 Selector
  - IconScrollCardTemplates.xaml
  - ImageTextScrollCardTemplate.xaml
  - PicTextMixTemplates.xaml 图文模板
- Models 数据模型
  - Entity.cs 包括所有数据模型的基类 实现了根据EntityType和EntityTemplate进行AutoCast
- Network 网络相关
  - CoolapkApi.\*.cs 酷安 Api
  - TokenHeaderHandler.cs Token 生成等
- Other 其他工具
  - AppUtil.cs
  - IncrementLoadingCollection.cs 实现了IncrementalLoadingEntityCollection
  - NotifyPropertyBase.cs
- Pages 各个页面
- Themes ...
- ViewModels 视图模型
  - BaseViewModel.cs 视图模型基类
- App.xaml


### 动态列表实现思路

[![大致意思](https://z3.ax1x.com/2021/04/14/cyb6Rx.md.png)](https://imgtu.com/i/cyb6Rx)

