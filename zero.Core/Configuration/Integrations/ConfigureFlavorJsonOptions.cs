using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace zero.Configuration;


public class ConfigureIntegrationJsonOptions : IConfigureOptions<JsonOptions>
{
  private readonly IZeroOptions zeroOptions;

  public ConfigureIntegrationJsonOptions(IZeroOptions options)
  {
    zeroOptions = options;
  }

  public void Configure(JsonOptions options)
  {
    options.JsonSerializerOptions.Converters.Add(new JsonIntegrationTypeVariantConverter(zeroOptions));
  }
}