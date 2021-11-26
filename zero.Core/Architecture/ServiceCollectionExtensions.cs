using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBlueprints(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IBlueprintService, BlueprintService>();
    services.AddScoped<IInterceptor, BlueprintInterceptor>();
    services.AddScoped<IInterceptor, BlueprintChildInterceptor>();

    services.AddOptions<BlueprintOptions>().Bind(config.GetSection("Zero:Blueprints"));

    return services;
  }
}