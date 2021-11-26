using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroApplications(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IApplicationResolver, ApplicationResolver>();
    services.AddScoped<IApplicationStore, ApplicationStore>();

    services.AddOptions<ApplicationOptions>().Bind(config.GetSection("Zero:Applications"));

    return services;
  }
}