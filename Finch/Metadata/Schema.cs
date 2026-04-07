//using Microsoft.AspNetCore.Mvc;
//using System.Dynamic;

//namespace Finch.Metadata;

//public class Schema
//{
//  public dynamic Model { get; private set; }

//  static JsonOptions Settings;

//  static Schema()
//  {
//    Settings = new JsonOptions()
//    {
//      Formatting = Formatting.None,
//      TypeNameHandling = TypeNameHandling.None
//    };
//    Settings.Converters.Add(new SchemaConverter());
//  }


//  public Schema(string type, Action<dynamic> set) : this(type, false, set) { }

//  public Schema(string type, bool isRoot, Action<dynamic> set)
//  {
//    Model = new ExpandoObject();
//    var dict = Model as IDictionary<string, object>;
//    if (isRoot)
//    {
//      dict["@context"] = "https://schema.org";
//    }
//    dict["@type"] = type;
//    set(Model);
//  }


//  public string ToJson() => JsonConvert.SerializeObject(Model, Settings);


//  class SchemaConverter : JsonConverter<Schema>
//  {
//    public override void WriteJson(JsonWriter writer, Schema value, JsonSerializer serializer)
//    {
//      serializer.Serialize(writer, value.Model);
//    }

//    public override Schema ReadJson(JsonReader reader, Type objectType, Schema existingValue, bool hasExistingValue, JsonSerializer serializer)
//    {
//      throw new NotImplementedException();
//    }
//  }
//}