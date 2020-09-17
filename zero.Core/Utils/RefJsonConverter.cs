using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Utils
{
  public class RefJsonConverter : JsonConverter
  {
    private readonly Type type;

    private readonly string idProperty;

    public RefJsonConverter()
    {
      type = typeof(Ref<>);
      idProperty = nameof(Ref<IZeroIdEntity>.Id);
    }


    public override bool CanRead => true;

    public override bool CanWrite => true;

    public override bool CanConvert(Type objectType) => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == type;


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      JToken t = JToken.FromObject(value);
      string id = t?.Value<string>(idProperty);

      if (t.Type != JTokenType.Object || id.IsNullOrEmpty())
      {
        writer.WriteNull();
      }
      else
      {
        writer.WriteValue(id);
      }
    }


    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (!(reader.Value is string))
      {
        return null;
      }

      if (((string)reader.Value).IsNullOrEmpty())
      {
        return null;
      }

      return Activator.CreateInstance(objectType, new object[1] { reader.Value });
    }
  }
}
