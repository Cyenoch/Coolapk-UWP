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

namespace Coolapk_UWP.Controls {
    public class AsyncLoadStateTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultLoadingTemplate { get; set; }
        public DataTemplate DefaultLoadedTemplate { get; set; }
        public DataTemplate DefaultExceptionTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            //return base.SelectTemplateCore(item, container);
            var Control = (AsyncLoadStateControl)item;
            if (Control.ErrorMessage != null && Control.ErrorMessage.Length > 0) {
                return DefaultExceptionTemplate;
            } else if (Control.IsLoading) {
                return DefaultLoadingTemplate;
            } else {
                var template = Control.DataLoadedTemplate ?? DefaultLoadedTemplate;
                return template;
            }
        }
    }

    public sealed partial class AsyncLoadStateControl : UserControl {
        public static DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(AsyncLoadStateControl), new PropertyMetadata(false, new PropertyChangedCallback(OnPropertyChanged)));
        public static DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(AsyncLoadStateControl), new PropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// 不同状态的模板选择方案，具有默认值
        /// 在Control.ErrorMessage != null && Control.ErrorMessage.Length > 0时使用DefaultExceptionTemplate，DataContext为AsyncLoadStateControl
        /// </summary>
        public DataTemplateSelector TemplateSelector { get; set; }

        /// <summary>
        /// 数据加载完成后的模板，DataContext默认
        /// </summary>
        public DataTemplate DataLoadedTemplate { get; set; }

        /// <summary>
        /// 错误页的重试按钮点击事件
        /// </summary>
        public event RoutedEventHandler Retry;

        public bool IsLoading {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        public string ErrorMessage {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public AsyncLoadStateControl() {
            InitializeComponent();
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                LoadContent();
            });
        }

        public void LoadContent() {
            if (TemplateSelector == null) TemplateSelector = (DataTemplateSelector)Resources["DefaultAsyncLoadStateTemplateSelector"];
            var tempContent = (FrameworkElement)TemplateSelector.SelectTemplate(this, this).LoadContent();
            // 仅在有错误的情况下，设置DataContext以便于错误页的信息展示
            if (ErrorMessage != null && ErrorMessage.Length > 0) tempContent.SetValue(FrameworkElement.DataContextProperty, this);
            Content = tempContent;
        }

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            AsyncLoadStateControl control = (AsyncLoadStateControl)d;
            control.LoadContent();
        }

        private void Retry_Click(object sender, RoutedEventArgs e) {
            Retry(sender, e);
        }
    }
}
