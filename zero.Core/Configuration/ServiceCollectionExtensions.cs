using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Configuration;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroConfiguration(this IServiceCollection services, IConfiguration config)
  {
    services.AddOptions<ZeroOptions>().Bind(config.GetSection("Zero")).Configure(opts => { });
    services.AddTransient<IZeroOptions>(factory => factory.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue);
    return services;
  }
}