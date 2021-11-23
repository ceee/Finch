using Microsoft.Extensions.DependencyInjection;

namespace zero.Context;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroContext(this IServiceCollection services)
  {
    services.AddScoped<IZeroContext, ZeroContext>();
    services.AddHttpContextAccessor();
    return services;
  }
}