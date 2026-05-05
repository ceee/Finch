using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mixtape.Models;

public class JsonFlavorVariantConverterFactory : JsonConverterFactory
{
  readonly IMixtapeOptions _options;

  public JsonFlavorVariantConverterFactory(IMixtapeOptions options)
  {
    _options = options;
  }

  public override bool CanConvert(Type typeToConvert)
  {
    if (!typeof(ISupportsFlavors).IsAssignableFrom(typeToConvert))
    {
      return false;
    }

    if (!_options.For<FlavorOptions>().Providers.TryGetValue(typeToConvert, out FlavorProvider provider) || provider.ConverterCreator == null)
    {
      return false;
    }

    return true;
  }

  public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
  {
    FlavorProvider provider = _options.For<FlavorOptions>().Providers[typeToConvert];
    return provider.ConverterCreator(provider);
  }
}