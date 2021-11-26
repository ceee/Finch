using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Spaces;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroSpaces(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<ISpaceStore, SpaceStore>();
    services.AddScoped<ISpaceTypeService, SpaceTypeService>();
    services.AddScoped<ISpaceService, SpaceService>();

    services.AddOptions<SpaceOptions>().Bind(config.GetSection("Zero:Spaces"));

    return services;
  }
}