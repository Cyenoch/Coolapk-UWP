using Coolapk_UWP.Other;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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

        [JsonIgnore]
        public ICollection<string> _picArr;

        public ICollection<string> PicArr {
            get {
                return _picArr;
            }
            set {
                // 酷安有时候会有一个 [""] 这样的坑壁操作
                _picArr = value.Where((pic) => pic.Length > 8).ToList();
            }
        }

        public bool HasPics {
            get {
                return PicArr != null && PicArr.Count > 0;
            }
        }
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

    public class FeedReply : Entity {
        public User UserInfo { get; set; }
        public uint ReplyRowsCount { get; set; }
        public bool IsFeedAuthor { get; set; }

        [JsonProperty("likenum")]
        public uint _likenum;

        [JsonIgnore]
        public uint Likenum {
            get {
                return _likenum;
            }
            set {
                Set(ref _likenum, value);
            }
        }

        [JsonProperty("rusername")]
        public string ReplyToUsername { get; set; }
        [JsonProperty("ruid")]
        public uint ReplyToUid { get; set; }

        /// <summary>
        /// 作为二级回复时，必定是一级回复的id
        /// </summary>
        [JsonProperty("rrid")]
        public uint RootReplyId { get; set; }

        /// <summary>
        /// 可能是二级回复的id
        /// </summary>
        [JsonProperty("rid")]
        public uint ReplyToReplyId { get; set; }

        public string Message { get; set; }
        public string InfoHtml { get; set; }

        public ObservableCollection<FeedReply> ReplyRows { get; set; }

        /// <summary>
        /// 是否有图片
        /// </summary>
        [JsonIgnore]
        public bool HasPic { get { return Pic != null && Pic.Length > 0; } }
        [JsonIgnore]
        public bool HasSubreply { get { return ReplyRows != null && ReplyRows.Count > 0; } }

        /// <summary>
        /// 是否是二级回复给一级回复
        /// </summary>
        [JsonIgnore]
        public bool IsReplyToRootReply { get { return ReplyToReplyId == RootReplyId && RootReplyId != 0; } }

    }
}
