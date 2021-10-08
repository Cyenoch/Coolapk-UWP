using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coolapk.WinUI.Controls.AdaptiveEntityList
{
    public class AdaptiveItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoAdapterCaseTmpl { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var tmpl = NoAdapterCaseTmpl;
            return tmpl;
        }
    }
}
