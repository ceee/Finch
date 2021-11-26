using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Configuration;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroConfiguration(this IServiceCollection services, IConfiguration config)
  {
    services.AddOptions<ZeroOptions>().Bind(config.GetSection("Zero")).Configure(opts =>
    {
      opts.Version = "0.0.1-alpha.1";
      opts.ZeroPath = "/zero";
      opts.TokenExpiration = TimeSpan.FromHours(3);
    });
    services.AddTransient<IZeroOptions>(factory => factory.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue);

    services.AddOptions<FeatureOptions>().Bind(config.GetSection("Zero:Features"));

    return services;
  }
}