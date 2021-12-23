using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace zero.Stores;


public class ConfigureFlavorJsonOptions : IConfigureOptions<JsonOptions>
{
  private readonly IZeroOptions zeroOptions;

  public ConfigureFlavorJsonOptions(IZeroOptions options)
  {
    zeroOptions = options;
  }

  public void Configure(JsonOptions options)
  {
    options.JsonSerializerOptions.Converters.Add(new JsonFlavorVariantConverterFactory(zeroOptions));
  }
}