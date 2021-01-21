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

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Lastupdate { get; set; } = DateTime.Now;

        virtual public IList<Entity> Entities { get; set; } = new List<Entity>();

        public T Cast<T>() where T : Entity {
            var _s = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<T>(_s);
        }

        public void Cast<T>(out T entity) where T : Entity {
            var _s = JsonConvert.SerializeObject(this);
            entity = JsonConvert.DeserializeObject<T>(_s);
        }

    }

    public class ConfigCard : Entity {

    }

    public class ImageCarouselCard : Entity {
        public double AspectHeight(double width) => width * (1080d / 360d);
    }

}
