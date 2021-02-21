using Coolapk_UWP.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Coolapk_UWP.ViewModels {
    public class CreateFeedViewModel : NotifyPropertyBase {
        public BitmapImage Cover { get; set; }
        public bool ShowCoverHint { get => Cover == null; }
        public bool HideCoverHint { get => !ShowCoverHint; }
        public Thickness AppBarHeight { get => new Thickness { Top = App.AppViewModel.AppBarHeight }; }

        public Dictionary<String, StorageFile> PicCache = new Dictionary<string, StorageFile> { };
    }
}
