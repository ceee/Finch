using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBlueprints(this IServiceCollection services)
  {
    services.AddScoped<IBlueprintService, BlueprintService>();
    services.AddScoped<IInterceptor, BlueprintInterceptor>();
    services.AddScoped<IInterceptor, BlueprintChildInterceptor>();
    return services;
  }
}