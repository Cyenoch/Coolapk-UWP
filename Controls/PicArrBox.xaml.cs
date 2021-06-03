using Coolapk_UWP.Models;
using Coolapk_UWP.Pages;
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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Coolapk_UWP.Controls
{
    public class FeedPicArrBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NonePicTemplate { get; set; }
        public DataTemplate OnePicTemplate { get; set; }
        public DataTemplate TwoColumnTemplate { get; set; }
        public DataTemplate ThreeColumnTemplate { get; set; }
        public DataTemplate FourGridTemplate { get; set; }
        public DataTemplate SixGridTemplate { get; set; }
        public DataTemplate NineGridTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            IList<string> picArr = (IList<string>)item;
            if (picArr.Count == 1) return OnePicTemplate;
            else if (picArr.Count == 2) return TwoColumnTemplate;
            else if (picArr.Count == 3) return ThreeColumnTemplate;
            else if (picArr.Count >= 4 && picArr.Count < 6) return FourGridTemplate;
            else if (picArr.Count >= 6 && picArr.Count < 9) return SixGridTemplate;
            else if (picArr.Count >= 9) return NineGridTemplate;
            else return NonePicTemplate;
        }
    }

    public sealed partial class PicArrBox : UserControl
    {
        public FeedPicArrBoxTemplateSelector FeedPicArrBoxTemplateSelector { get; set; }
        public static DependencyProperty PicArrProperty = DependencyProperty.Register(
            "PicArr",
            typeof(IList<string>),
            typeof(PicArrBox),
            new PropertyMetadata(
                new List<string>(),
                new PropertyChangedCallback(OnPicArrChanged)));

        public static DependencyProperty SourcePicArrProperty = DependencyProperty.Register(
            "SourcePicArr",
            typeof(IList<string>),
            typeof(PicArrBox),
            new PropertyMetadata(
                new List<string>(),
                new PropertyChangedCallback(OnSourcePicArrChanged)));

        public IList<string> PicArr
        {
            get => (IList<string>)GetValue(PicArrProperty);
            set => SetValue(PicArrProperty, value);
        }
        public IList<string> SourcePicArr
        {
            get => (IList<string>)GetValue(SourcePicArrProperty);
            set => SetValue(SourcePicArrProperty, value);
        }
        public PicArrBox()
        {
            InitializeComponent();
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                LoadContent();
            });
        }
        public void LoadContent()
        {
            if (FeedPicArrBoxTemplateSelector == null)
            {
                FeedPicArrBoxTemplateSelector = (FeedPicArrBoxTemplateSelector)Resources["FeedPicArrBoxTemplateSelector"];
            }

            if (PicArr != null && PicArr.Count > 0 && FeedPicArrBoxTemplateSelector != null)
            {
                FrameworkElement ele = (FrameworkElement)(FeedPicArrBoxTemplateSelector.SelectTemplate(PicArr, this)?.LoadContent());
                ele.SetValue(FrameworkElement.DataContextProperty, this);
                ele.Tapped += Ele_Tapped;
                Content = ele;
            }
        }

        private void Ele_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PhotoGralley.Navigate(SourcePicArr ?? PicArr);
            e.Handled = true;
        }

        private static void OnPicArrChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PicArrBox b = d as PicArrBox;
            b.LoadContent();
        }

        private static void OnSourcePicArrChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var b = d as PicArrBox;
            //b.LoadContent();
        }
    }
}
