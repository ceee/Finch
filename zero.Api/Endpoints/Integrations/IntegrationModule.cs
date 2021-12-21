using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Integrations;

public class IntegrationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IMapperProfile, IntegrationMapperProfile>();

    //services.Configure<RavenOptions>(opts =>
    //{
    //  opts.Indexes.Add<zero_Api_Languages_Listing>();
    //});
  }
}