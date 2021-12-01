using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Translations;

public class TranslationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, TranslationPermissions>();
    services.AddSingleton<IMapperProfile, TranslationMapperProfile>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Api_Translations_Listing>();
    });
  }
}