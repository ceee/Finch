using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroApplications(this IServiceCollection services)
  {
    services.AddScoped<IApplicationResolver, ApplicationResolver>();
    return services;
  }
}