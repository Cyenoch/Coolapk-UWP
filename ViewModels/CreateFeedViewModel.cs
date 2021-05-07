using Coolapk_UWP.Models;
using Coolapk_UWP.Other;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Coolapk_UWP.ViewModels
{
    public class CreateFeedViewModel : BaseViewModel
    {
        public BitmapSource _cover;
        public BitmapSource Cover { get => _cover; set => Set(ref _cover, value); }
        public StorageFile CoverSourceFile { get; set; }
        public bool ShowCoverHint { get => Cover == null; }
        public bool HideCoverHint { get => !ShowCoverHint; }
        public Thickness AppBarHeight { get => new Thickness { Top = App.AppViewModel.AppBarHeight }; }

        public Dictionary<String, StorageFile> PicCache = new Dictionary<string, StorageFile> { };

        public async Task CreateHtmlArticleFeed(IList<MessageRawStructBase> messageStruct, string title)
        {
            try
            {
                var resp = await this.CoolapkApis.CreateHtmlArticleFeed(
                    message: JsonConvert.SerializeObject(messageStruct, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    }),
                    messageTitle: title);
            }
            catch (Exception err)
            {
                Console.Write(err.StackTrace);
            }
            Console.Write("??");
            return;
        }
    }
}
