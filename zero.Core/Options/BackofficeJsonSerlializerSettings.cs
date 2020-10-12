using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using zero.Core.Utils;

namespace zero.Core.Options
{
  public class BackofficeJsonSerlializerSettings : JsonSerializerSettings
  {
    public BackofficeJsonSerlializerSettings() : this(false) { }

    public BackofficeJsonSerlializerSettings(bool typed)
    {
      Setup(this, typed);
    }


    public static void Setup(JsonSerializerSettings settings, bool typed = false)
    {
      settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
      settings.Converters.Add(new RefJsonConverter());
      settings.ContractResolver = new ZeroJsonContractResolver();
      settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      settings.TypeNameHandling = TypeNameHandling.Objects;
      //settings.SerializationBinder = new ZeroInterfaceBinder();
    }
  }
}
