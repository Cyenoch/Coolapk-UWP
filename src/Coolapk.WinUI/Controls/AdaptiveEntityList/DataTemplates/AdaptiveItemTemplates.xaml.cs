using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Coolapk.WinUI.Controls.AdaptiveEntityList.DataTemplates
{
    public sealed partial class AdaptiveItemTemplates : ResourceDictionary
    {
        public AdaptiveItemTemplates()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ele = (e.OriginalSource as FrameworkElement);
        }
    }
}
