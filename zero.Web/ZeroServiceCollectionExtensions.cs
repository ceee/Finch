using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core.Options;

namespace zero.Web
{
  public static class ZeroServiceCollectionExtensions
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
}