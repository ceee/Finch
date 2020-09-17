using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sparrow.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Utils
{
  public class RefsJsonConverter : JsonConverter
  {
    private readonly Type type;

    private readonly string idProperty;

    public RefsJsonConverter()
    {
      type = typeof(Refs<>);
      idProperty = nameof(Refs<IZeroIdEntity>.Ids);
    }


    public override bool CanRead => true;

    public override bool CanWrite => true;

    public override bool CanConvert(Type objectType) => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == type;


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      JToken t = JToken.FromObject(value);
      JArray idArray = t?.Value<JArray>(idProperty);
      string[] ids = idArray.Values<string>().ToArray();

      writer.WriteStartArray();

      if (ids?.Length > 0)
      {
        foreach (string id in ids)
        {
          writer.WriteValue(id);
        }    
      }

      writer.WriteEndArray();
    }


    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.StartArray)
      {
        JArray array = JArray.Load(reader);
        string[] items = array.Select(x => x.Value<string>()).ToArray();
        return Activator.CreateInstance(objectType, new object[1] { items });  
      }

      return Activator.CreateInstance(objectType);
    }
  }
}
