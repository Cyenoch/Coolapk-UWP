using Coolapk_UWP.Other;
using Newtonsoft.Json;
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

    public class FeedDetail : Feed {
        /// <summary>
        /// 仅FeedCover该字段才不是null
        /// </summary>
        [JsonProperty("message_raw_output")]
        [JsonConverter(typeof(MessageRawConverter))]
        public ICollection<MessageRawStructBase> MessageRaw { get; set; }

        [JsonProperty("message_cover")]
        public string Cover { get; set; }
    }
}
