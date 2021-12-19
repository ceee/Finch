using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Pages;

public class PageModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, PagePermissions>();
    services.AddSingleton<IMapperProfile, PageMapperProfile>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Api_Pages_Listing>();
      opts.Indexes.Add<zero_Api_Pages_ChildCounts>();
    });
  }
}