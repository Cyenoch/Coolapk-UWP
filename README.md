# Coolapk-UWP

**这是一个兴趣使然的项目，主要目的用于学习 UWP 应用开发，以及 C#语言**

其次，为了方便在 Windows10(包括未来的 Windows X) 上刷酷安

> **仅供个人用于学习、研究;不得用于商业用途;**

### 预览效果

#### 首页响应式布局效果

[![主页显示效果1](https://z3.ax1x.com/2021/04/13/cylYef.png)](https://imgtu.com/i/cylYef)
[![主页显示效果2](https://z3.ax1x.com/2021/04/13/cyltw8.md.png)](https://imgtu.com/i/cyltw8)
[![主页显示效果3](https://z3.ax1x.com/2021/04/13/cylNTS.md.png)](https://imgtu.com/i/cylNTS)

#### 图文动态显示

[![图文动态显示效果](https://z3.ax1x.com/2021/04/13/cyQax1.md.png)](https://imgtu.com/i/cyQax1)

#### 图文编辑器

[![图文编辑器效果](https://z3.ax1x.com/2021/04/13/cyQ2RA.md.png)](https://imgtu.com/i/cyQ2RA)

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
  - Entity.cs 包括所有数据模型的基类
- Network 网络相关
  - CoolapkApi.\*.cs 酷安 Api
  - TokenHeaderHandler.cs Token 生成等
- Other 其他工具
  - AppUtil.cs
  - IncrementLoadingCollection.cs
  - NotifyPropertyBase.cs
- Pages 各个页面
- Themes ...
- ViewModels 视图模型
  - BaseViewModel.cs 视图模型基类
- App.xaml
