using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace zero.Stores;

public class ConfigureModuleTypeJsonOptions : IConfigureOptions<JsonOptions>
{
  private readonly IZeroOptions zeroOptions;

  public ConfigureModuleTypeJsonOptions(IZeroOptions options)
  {
    zeroOptions = options;
  }

  public void Configure(JsonOptions options)
  {
    options.JsonSerializerOptions.Converters.Add(new JsonModuleTypeConverter(zeroOptions));
  }
}