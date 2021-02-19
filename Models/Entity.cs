using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Coolapk_UWP.Other;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coolapk_UWP.Models {
    public class Entity : NotifyPropertyBase {
        [JsonExtensionData]
        public IDictionary<string, JToken> OtherField { get; set; }

        [JsonProperty(
            PropertyName = "entityId"
         )]
        [JsonConverter(typeof(StringToIntConverter))]
        public uint EntityID { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string EntityType { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string EntityTemplate { get; set; }

        public string Title { get; set; }// 可空
        public string Url { get; set; }// 
        public string Pic { get; set; }// 可空
        public string Logo { get; set; } //

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Lastupdate { get; set; } = DateTime.Now;

        virtual public IList<Entity> Entities { get; set; } = new List<Entity>();

        public T Cast<T>() where T : Entity {
            // 根据目标类型重新解析实体，OtherField包含了Entity未解析的字段
            var _s = JsonConvert.SerializeObject(this);
            var entity = JsonConvert.DeserializeObject<T>(_s);
            // 遍历Entities然后转换类型 重新解析
            if (entity.Entities != null && entity.Entities.Count > 0) {
                var temp = new Entity[entity.Entities.Count()];
                entity.Entities.CopyTo(temp, 0);
                entity.Entities.Clear();
                foreach (var child in temp.OfType<Entity>()) {
                    entity.Entities.Add(child.AutoCast() as Entity);
                }
            }
            return entity;
        }

        [JsonIgnore]
        public string JsonString {
            get {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }

        [JsonIgnore]
        public string HumanReadableDateString {
            get { return AppUtil.GetReadDateString(Lastupdate); }
        }

        public void Cast<T>(out T entity) where T : Entity => entity = Cast<T>();

        public object AutoCast() {
            switch (EntityType) {
                case "user":
                    return Cast<User>();
                case "product":
                    return Cast<Product>();
                case "feed_reply":
                    return Cast<FeedReply>();
                case "apk":
                    return Cast<Apk>();
                case "feed":
                    switch (EntityTemplate) {
                        case "feed":
                            return Cast<Feed>();
                        case "feedCover":
                            return Cast<FeedCover>();
                    }
                    break;
                case "card":
                    switch (EntityTemplate) {
                        case "iconLinkGridCard":
                            return Cast<IconLinkGridCard>();
                        case "configCard":
                            return Cast<ConfigCard>();
                        case "imageCarouselCard_1":
                            return Cast<ImageCarouselCard>();
                        case "iconScrollCard":
                            return Cast<IconScrollCard>();
                        case "titleCard":
                            return Cast<TitleCard>();
                        case "apkImageCard":
                            return Cast<IgnoreCard>();
                        case "imageTextScrollCard":
                            return Cast<ImageTextScrollCard>();
                    }
                    break;
                case "album":
                    switch (EntityTemplate) {
                        case "albumExpandCardTopCover": // 应用集
                            return Cast<IgnoreCard>();
                    }
                    break;
            }
            return this;
        }
    }

    public class ActionEntity : Entity {
        [JsonProperty("likenum")]
        public uint _likenum;

        [JsonIgnore]
        public bool ShowLikeNum { get { return _likenum > 0; } }

        [JsonIgnore]
        public uint Likenum { get { return _likenum; } set { Set(ref _likenum, value); } }

        public UserAction UserAction { get; set; }

        public async Task<Resp<LikeActionResult>> ToggleLike() {
            Resp<LikeActionResult> resp;
            var old = UserAction.Like;
            try {
                if (UserAction.Like) {
                    UserAction.Like = false;
                    resp = await App.AppViewModel.CoolapkApis.DoUnLike(this.EntityID);
                } else {
                    UserAction.Like = true;
                    resp = await App.AppViewModel.CoolapkApis.DoLike(this.EntityID);
                }
                if (resp.Message != null) throw new Exception(resp.Message);
                Likenum = resp.Data.Count;
                return resp;
            } catch (Exception err) {
                UserAction.Like = old;
                throw err;
            }
        }
    }

    // 比如 新鲜图文
    public class ImageTextScrollCard : Entity {
        // href to datalist => URL + Title
    }

    /// <summary>
    /// 不会做适配或是广告内容
    /// </summary>
    public class IgnoreCard : Entity { }

    public class IconLinkGridCard : Entity { }

    public class TitleCard : Entity { }

    public class ConfigCard : Entity { }

    public class ImageCarouselCard : Entity { }

    public class IconScrollCard : Entity { }

    public class Apk : Entity { }

    public class Product : Entity { }

    public class MainInit {
        public string Title;
        public string Icon;
        [JsonProperty("entities")]
        public IList<MainInitTabConfig> Tabs;
    }
    public class MainInitTabConfig {
        public string Title;
        [JsonProperty("page_name")]
        public string PageName;
        public string Url;
        public string Logo;
        [JsonProperty("entities")]
        public IList<MainInitTabConfig> SubTabs;
    }
}
