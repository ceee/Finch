using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero;

public static class ServiceCollectionExtensions
{
  public static ZeroBuilder AddZero(this IServiceCollection services, IConfiguration configuration)
  {
    return new ZeroBuilder(services, configuration, null);
  }

  public static ZeroBuilder AddZero(this IServiceCollection services, IConfiguration configuration, Action<IZeroStartupOptions> setupAction)
  {
    return new ZeroBuilder(services, configuration, setupAction);
  }
}