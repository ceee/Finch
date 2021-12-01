using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Countries;

public class CountryModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, CountryPermissions>();
    services.AddSingleton<IMapperProfile, CountryMapperProfile>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Api_Countries_Listing>();
    });
  }
}