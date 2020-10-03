//using Newtonsoft.Json;
//using System;
//using zero.Core.Entities;
//using zero.Core.Extensions;

//namespace zero.Core.Utils
//{
//  public class ZeroEntityJsonConverter : JsonConverter
//  {
//    private readonly Type type;

//    private readonly string idProperty;

//    public ZeroEntityJsonConverter()
//    {
//      type = typeof(Ref<>);
//      idProperty = nameof(Ref<IZeroIdEntity>.Id);
//    }


//    public override bool CanRead => true;

//    public override bool CanWrite => true;

//    public override bool CanConvert(Type objectType) => objectType.IsInterface && objectType.GetGenericTypeDefinition() == type;


//    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    {
//      string id = null;

//      if (value is Ref)
//      {
//        id = (value as Ref).Id;
//      }

//      if (id.IsNullOrEmpty())
//      {
//        writer.WriteNull();
//      }
//      else
//      {
//        writer.WriteValue(id);
//      }
//    }


//    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//    {
//      if (!(reader.Value is string))
//      {
//        return null;
//      }

//      if (((string)reader.Value).IsNullOrEmpty())
//      {
//        return null;
//      }

//      return Activator.CreateInstance(objectType, new object[1] { reader.Value });
//    }
//  }
//}
