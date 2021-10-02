using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coolapk.Common.JsonConverters
{
    public class StringToIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string) || objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // 在为null时返回0
            object value = reader.Value;
            switch (value)
            {
                case int intV:
                    return intV;
                case string stringV:
                    return uint.Parse(stringV);
                default:
                    return value == null ? 0 : (object)uint.Parse(value.ToString());
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
