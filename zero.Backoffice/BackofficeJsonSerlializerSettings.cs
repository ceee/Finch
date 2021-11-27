using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace zero.Backoffice;

internal class BackofficeJsonSerlializerSettings : JsonSerializerSettings
{
  public BackofficeJsonSerlializerSettings()
  {
    Setup(this);
  }


  public static void Setup(JsonSerializerSettings settings)
  {
    settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
    //settings.Converters.Add(new RefJsonConverter());
    //settings.ContractResolver = new ZeroJsonContractResolver();
    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    settings.TypeNameHandling = TypeNameHandling.Objects;
    //settings.SerializationBinder = new ZeroInterfaceBinder();
  }
}