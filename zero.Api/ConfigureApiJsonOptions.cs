using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace zero.Api;


public class ConfigureApiJsonOptions : IConfigureOptions<JsonOptions>
{
  public void Configure(JsonOptions options)
  {
    // TODO this matches all serialization, not limited to API
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
  }
}