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
using Coolapk_UWP.Models;


namespace Coolapk_UWP.DataTemplates {
    public class PicTextMixTemplateSelector : DataTemplateSelector {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate ImageTempalte { get; set; }
        public DataTemplate UnsupportTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            switch (item) {
                case MessageRawStructText _:
                    return TextTemplate;
                case MessageRawStructImage _:
                    return ImageTempalte;
                case MessageRawStructUnsupport _: // default
                default:
                    return UnsupportTemplate;
            }
        }
    }

    public sealed partial class PicTextMixTemplates : ResourceDictionary {
        public PicTextMixTemplates() {
            this.InitializeComponent();
        }
    }
}
