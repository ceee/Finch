using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Finch.Models;


public class ConfigureFlavorJsonOptions : IConfigureOptions<JsonOptions>
{
  private readonly IFinchOptions _finchOptions;

  public ConfigureFlavorJsonOptions(IFinchOptions options)
  {
    _finchOptions = options;
  }

  public void Configure(JsonOptions options)
  {
    options.JsonSerializerOptions.Converters.Add(new JsonFlavorVariantConverterFactory(_finchOptions));
  }
}