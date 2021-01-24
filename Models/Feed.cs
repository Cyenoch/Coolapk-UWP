using Coolapk_UWP.Other;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Models {
    public enum FeedType {
        Unknow = -1,
        Default = 0,
        HtmlArticle = 12,
    }

    public class Feed : Entity {
        public string Message { get; set; }

        [JsonProperty("is_html_article")]
        [JsonConverter(typeof(JsonIntToBoolConverter))]
        public bool IsHtmlArticle { get; set; }

        [JsonConverter(typeof(JsonIntToBoolConverter))]
        public bool IsStickTop { get; set; }

        // feedArticle feed
        public string FeedType { get; set; }

        // 重要
        [JsonConverter(typeof(FeedTypeConverter))]
        public FeedType Type { get; set; }
    }

    public class FeedCover : Feed {
    }
}
