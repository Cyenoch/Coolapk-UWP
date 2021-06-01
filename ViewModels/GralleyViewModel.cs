using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.ViewModels
{
    public class GralleyViewModel : BaseViewModel
    {
        private IList<string> photos;
        public IList<string> Photos
        {
            get => photos; set => Set(ref photos, value);
        }

    }
}
