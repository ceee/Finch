using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Users;

public class UserModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, UserPermissions>();
    services.AddSingleton<IMapperProfile, UserMapperProfile>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Api_Users_Listing>();
    });
  }
}