using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mixtape.Models;


public class ConfigureFlavorJsonOptions : IConfigureOptions<JsonOptions>
{
  private readonly IMixtapeOptions _mixtapeOptions;

  public ConfigureFlavorJsonOptions(IMixtapeOptions options)
  {
    _mixtapeOptions = options;
  }

  public void Configure(JsonOptions options)
  {
    options.JsonSerializerOptions.Converters.Add(new JsonFlavorVariantConverterFactory(_mixtapeOptions));
  }
}