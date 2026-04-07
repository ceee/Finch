using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch;

public static class ServiceCollectionExtensions
{
  public static FinchBuilder AddFinch(this IServiceCollection services, IConfiguration configuration)
  {
    return new FinchBuilder(services, configuration, null);
  }

  public static FinchBuilder AddFinch(this IServiceCollection services, IConfiguration configuration, Action<IFinchStartupOptions> setupAction)
  {
    return new FinchBuilder(services, configuration, setupAction);
  }
}