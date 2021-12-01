using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Applications;

public class ApplicationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, ApplicationPermissions>();
    services.AddSingleton<IMapperProfile, ApplicationMapperProfile>();
  }
}