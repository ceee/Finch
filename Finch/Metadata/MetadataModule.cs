using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Metadata;

public class FinchMetadataModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IMetadataService, MetadataService>();
    services.AddHttpClient<AnalyticsController>();
    services.AddOptions<AnalyticsOptions>().Bind(configuration.GetSection("Finch:Analytics"));
  }
}