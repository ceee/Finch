using System.Text.Json;
using System.Text.Json.Serialization;

namespace Finch.Models;

internal class JsonFlavorVariantConverter<T> : JsonDiscriminatorConverter<T> where T : class, ISupportsFlavors, new()
{
  protected FlavorProvider Provider { get; }

  Type _factoryType = typeof(JsonFlavorVariantConverterFactory);


  public JsonFlavorVariantConverter(FlavorProvider provider) : base("flavor")
  {
    Provider = provider;
  }


  protected override Type GetTypeFromDiscriminator(Type requestedType, string discriminator)
  {
    FlavorConfig config = Provider.Flavors.FirstOrDefault(x => x.Alias.Equals(discriminator, StringComparison.InvariantCultureIgnoreCase));
    return config?.FlavorType ?? Provider.FlavorlessType ?? requestedType;
  }


  protected override JsonSerializerOptions CreateOptions(JsonSerializerOptions baseOptions)
  {
    JsonSerializerOptions newOptions = base.CreateOptions(baseOptions);
    JsonConverter toRemove = newOptions.Converters.FirstOrDefault(x => x.GetType() == _factoryType);
    newOptions.Converters.Remove(toRemove);
    return newOptions;
  }
}