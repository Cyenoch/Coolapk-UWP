using Coolapk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Coolapk.Api.Converters
{
    /// <summary>
    /// 转换FeedType
    /// </summary>
    public class FeedTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return FeedType.Default;

            try
            {
                var e = Enum.Parse(typeof(FeedType), Enum.GetName(typeof(FeedType), reader.Value));
                return e;
            }
            catch (Exception _)
            {
                return FeedType.Unknow;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((int)value);
        }
    }

    // 下面是json的converter
    /// <summary>
    /// 转换MessageRaw
    /// </summary>
    public class MessageRawConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new Collection<MessageRawStructBase>();
            var value = reader.Value?.ToString();
            if (value == null || value == "null") return result;
            try
            {
                var baseStruct = JsonConvert.DeserializeObject<MessageRawStructBase[]>(value);
                foreach (var child in baseStruct)
                {
                    result.Add(child.AutoCast());
                }
            }
            catch (Exception _)
            {
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
