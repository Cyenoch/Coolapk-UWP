using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using System.IO;
using Newtonsoft.Json;

namespace Coolapk_UWP.Other
{
    public class EmojiMapStructParent
    {
        [JsonProperty(
            PropertyName = "map"
        )]
        public IList<EmojiMapStruct> Map { get; set; }
    }
    public class EmojiMapStruct
    {
        [JsonProperty(
            PropertyName = "key"
         )]
        public string Key { get; set; }
        [JsonProperty(
            PropertyName = "value"
        )]
        public string Value { get; set; }
    }
    public class EmojisUtil
    {
        //public static readonly ResourceLoader EmojiIdLoader = ResourceLoader.GetForViewIndependentUse("EmojiId");
        static EmojiMapStructParent emojiMapSource;
        static public Dictionary<string, string> EmojiMap = new Dictionary<string, string>();
        static public List<string> Emojis;
        static public Uri GetEmojiUriFor(string name)
        {
            return new Uri(@"ms-appx:///Assets/Emoji/" + EmojiMap[name] + ".png");
        }
        static public async void LoadEmojisResw()
        {
            try
            {
                if (emojiMapSource == null)
                {
                    var emojiMapFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/emojimap.json"));
                    string jsonText = Windows.Storage.FileIO.ReadTextAsync(emojiMapFile).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    emojiMapSource = Newtonsoft.Json.JsonConvert.DeserializeObject<EmojiMapStructParent>(jsonText);
                    Emojis = emojiMapSource.Map.Select(m =>
                    {
                        EmojiMap[m.Key] = m.Value;
                        return m.Key;
                    }).ToList();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
