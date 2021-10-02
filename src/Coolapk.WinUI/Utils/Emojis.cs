using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Coolapk.WinUI.Utils.Emoji
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
        private static EmojiMapStructParent EmojiMapSource;
        public static Dictionary<string, string> EmojiMap = new Dictionary<string, string>();
        public static List<string> Emojis;
        public static Uri GetEmojiUriFor(string name)
        {
            return new Uri(@"ms-appx:///Assets/Emoji/" + EmojiMap[name] + ".png");
        }
        public static async void LoadEmojisResw()
        {
            try
            {
                if (EmojiMapSource == null)
                {
                    Windows.Storage.StorageFile emojiMapFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/emojimap.json"));
                    string jsonText = Windows.Storage.FileIO.ReadTextAsync(emojiMapFile).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    EmojiMapSource = JsonConvert.DeserializeObject<EmojiMapStructParent>(jsonText);
                    Emojis = EmojiMapSource.Map.Select(m =>
                    {
                        EmojiMap[m.Key] = m.Value;
                        return m.Key;
                    }).ToList();
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
