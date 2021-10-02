using Coolapk.Api.Converters;
using Coolapk.Common.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Coolapk.Models
{
    public enum FeedType
    {
        Unknow = -1,
        Default = 0,
        HtmlArticle = 12,
    }

    public class Feed : ActionEntity
    {

        public User UserInfo { get; set; }

        [JsonIgnore]
        public string MessageWithUserSpaceLink
        {
            get
            {
                return $"<a href=\"/u/{UserInfo.Uid}\">@{UserInfo.Username}:</a>" + Message;
            }
        }
        [JsonIgnore]
        public string _cachedMessageWithEmojiJoined = "";
        //[JsonIgnore]
        //public string MessageWithEmoji
        //{
        //    get
        //    {
        //        if (_cachedMessageWithEmojiJoined.Equals(string.Empty))
        //        {
        //            var msg = Message;
        //            EmojisUtil.Emojis.ForEach(emoji =>
        //            {
        //                msg = msg.Replace(emoji, "<emoji src=\"" + EmojisUtil.GetEmojiUriFor(emoji) + "\" />");
        //            });
        //            return msg;
        //        }
        //        return _cachedMessageWithEmojiJoined;
        //    }
        //}
        public string Message { get; set; }
        public string InfoHtml { get; set; }

        [JsonProperty("is_html_article")]
        [JsonConverter(typeof(JsonIntToBoolConverter))]
        public bool IsHtmlArticle { get; set; }

        [JsonConverter(typeof(JsonIntToBoolConverter))]
        public bool IsStickTop { get; set; }

        [JsonProperty("device_title")]
        public string DeviceTitle { get; set; }

        // 已知值 feedArticle feed comment (评论某个应用啥的，作为一个评论存在)
        public string FeedType { get; set; }

        // 重要
        [JsonConverter(typeof(FeedTypeConverter))]
        public FeedType Type { get; set; }

        [JsonIgnore]
        public IList<string> _picArr;
        [JsonIgnore]
        public IList<string> _smallPicArr;

        [JsonIgnore]
        public IList<string> SmallPicArr { get { return _smallPicArr; } }

        public IList<string> PicArr
        {
            get { return _picArr; }
            set
            {
                // 酷安有时候会有一个 [""] 这样的坑壁操作
                _picArr = value.Where((pic) => pic.Length > 8).ToList();
                _smallPicArr = _picArr.Select(pic => pic.EndsWith(".gif") ? pic : (pic + ".s.jpg")).ToList();
            }
        }

        [JsonIgnore]
        public bool HasPics
        {
            get
            {
                return PicArr != null && PicArr.Count > 0;
            }
        }

        [JsonProperty("replynum")]
        public uint _replynum;

        //[JsonIgnore]
        //public uint ReplyNum { get { return _replynum; } set { Set(ref _replynum, value); } }

        [JsonIgnore]
        public bool ShowReplyNum { get { return _replynum > 0; } }

        [JsonIgnore]
        public bool HasForwardFeed { get { return ForwardSourceFeed != null; } }

        // 转发的feed
        public Feed ForwardSourceFeed { get; set; }

        // 已知 "feed"
        public string ForwardSourceType { get; set; }

        public IList<RelationRow> RelationRows { get; set; }
    }

    public class FeedCover : Feed
    {
    }

    public class FeedDetail : Feed
    {
        /// <summary>
        /// 仅FeedCover该字段才不是null
        /// </summary>
        [JsonProperty("message_raw_output")]
        [JsonConverter(typeof(MessageRawConverter))]
        public ICollection<MessageRawStructBase> MessageRaw { get; set; }

        [JsonIgnore]
        public ICollection<MessageRawStructBase> MessageRawWithFeed { get => MessageRaw.Select(r => { r.ParentFeed = this; return r; }).ToArray(); }

        [JsonProperty("message_cover")]
        public string Cover { get; set; }
    }

    public class FeedReply : ActionEntity
    {
        public User UserInfo { get; set; }
        public uint ReplyRowsCount { get; set; }
        public bool IsFeedAuthor { get; set; }

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

        public IList<RelationRow> RelationRows { get; set; }

        [JsonIgnore]
        public string _cachedMessageWithEmojiJoined = "";
        //[JsonIgnore]
        //public string MessageWithEmoji
        //{
        //    get
        //    {
        //        if (_cachedMessageWithEmojiJoined.Equals(String.Empty))
        //        {
        //            var msg = Message;
        //            EmojisUtil.Emojis.ForEach(emoji =>
        //            {
        //                msg = msg.Replace(emoji, "<emoji src=\"" + EmojisUtil.GetEmojiUriFor(emoji) + "\" />");
        //            });
        //            return msg;
        //        }
        //        return _cachedMessageWithEmojiJoined;
        //    }
        //}
    }

    /// <summary>
    /// Logo Title Url EntityType
    /// </summary>
    public class RelationRow : Entity { }

    public class UserAction
    {
        public bool _like = false;
        public bool _favorite = false;
        public bool _follow = false;
        public bool _collect = false;

        //[JsonConverter(typeof(JsonIntToBoolConverter))]
        //public bool Like { get { return _like; } set { Set(ref _like, value); OnPropertyChanged("LikeButtonColor"); } }

        //[JsonConverter(typeof(JsonIntToBoolConverter))]
        //public bool Favorite { get { return _favorite; } set { Set(ref _favorite, value); } }

        //[JsonConverter(typeof(JsonIntToBoolConverter))]
        //public bool Follow { get { return _follow; } set { Set(ref _follow, value); } }

        //[JsonConverter(typeof(JsonIntToBoolConverter))]
        //public bool Collect { get { return _collect; } set { Set(ref _collect, value); } }

        //public Windows.UI.Xaml.Media.SolidColorBrush LikeButtonColor
        //{
        //    get
        //    {
        //        return ((Windows.UI.Xaml.Media.SolidColorBrush)App.Current.Resources[Like ? "SystemColorControlAccentBrush" : "SystemColorGrayTextBrush"]);
        //    }
        //}
    }
}
