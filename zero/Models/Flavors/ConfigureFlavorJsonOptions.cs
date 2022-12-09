using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace zero.Models;


public class ConfigureFlavorJsonOptions : IConfigureOptions<JsonOptions>
{
  private readonly IZeroOptions _zeroOptions;

  public ConfigureFlavorJsonOptions(IZeroOptions options)
  {
    _zeroOptions = options;
  }

  public void Configure(JsonOptions options)
  {
    options.JsonSerializerOptions.Converters.Add(new JsonFlavorVariantConverterFactory(_zeroOptions));
  }
}