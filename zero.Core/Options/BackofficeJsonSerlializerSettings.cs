using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace zero.Core.Options
{
  public class BackofficeJsonSerlializerSettings : JsonSerializerSettings
  {
    public BackofficeJsonSerlializerSettings() : this(false) { }

    public BackofficeJsonSerlializerSettings(bool typed)
    {
      Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'" });
      Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
      ContractResolver = new CamelCasePropertyNamesContractResolver();
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      TypeNameHandling = typed ? TypeNameHandling.Objects : TypeNameHandling.None;
    }
  }
}
