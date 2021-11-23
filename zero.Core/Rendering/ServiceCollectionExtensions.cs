using Microsoft.Extensions.DependencyInjection;

namespace zero.Rendering;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroRendering(this IServiceCollection services)
  {
    services.AddScoped<IRazorRenderer, RazorRenderer>();
    return services;
  }
}