using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Spaces;

public class SpacesModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<ISpaceStore, SpaceStore>();
    services.AddScoped<ISpaceTypeService, SpaceTypeService>();
    services.AddScoped<ISpaceService, SpaceService>();

    services.Configure<FlavorOptions>(opts =>
    {
      opts.Configure<Space>(cfg =>
      {
        cfg.CanUseWithoutFlavors = false;
      });
    });
  }
}