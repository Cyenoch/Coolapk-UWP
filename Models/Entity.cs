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
    public class Entity {
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
                foreach (var child in temp) {
                    entity.Entities.Add(child.AutoCast() as Entity);
                }
            }
            return entity;
        }

        public void Cast<T>(out T entity) where T : Entity => entity = Cast<T>();

        public object AutoCast() {
            switch (EntityType) {
                case "user":
                    return Cast<User>();
                case "product":
                    // 暂时默认
                    break;
                case "apk":
                    return Cast<Apk>();
                case "feed":
                    switch (EntityTemplate) {
                        case "feed":
                            return Cast<Feed>();
                        case "feedCover":
                            return Cast<FeedCover>();
                        default:
                            break;
                    }
                    break;
                case "card":
                    switch (EntityTemplate) {
                        case "configCard":
                            return Cast<ConfigCard>();
                        case "imageCarouselCard_1":
                            return Cast<ImageCarouselCard>();
                        case "iconScrollCard":
                            return Cast<IconScrollCard>();
                        case "titleCard":
                            return Cast<TitleCard>();
                    }
                    break;
            }
            return this;
        }
    }
    public class TitleCard : Entity { }

    public class ConfigCard : Entity { }

    public class ImageCarouselCard : Entity { }

    public class IconScrollCard : Entity { }

    public class Apk : Entity { }

}
