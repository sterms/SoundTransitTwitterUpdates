using System;
using Newtonsoft.Json;

namespace SoundTransitTwitterUpdates.Requests
{
    public class AlexaDateTimeJsonConverter : JsonConverter
    {
        public const string AlexaTimeFormat = "yyyy-MM-dd'T'HH:mm:ss'.0Z'";

        public override bool CanConvert(Type objectType)
        {
            return (objectType is DateTime || objectType is DateTimeOffset);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTimeOffset.ParseExact(existingValue.ToString(), AlexaTimeFormat, null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((DateTimeOffset)value).ToString(AlexaTimeFormat));
        }
    }
}
