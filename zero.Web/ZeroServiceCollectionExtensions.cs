using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core;

namespace zero.Web
{
  public static class ZeroServiceCollectionExtensions
  {
    public static ZeroBuilder AddZero(this IServiceCollection services)
    {
      return new ZeroBuilder(services);
    }

    public static ZeroBuilder AddZero(this IServiceCollection services, Action<ZeroOptions> setupAction)
    {
      return services.AddZero().WithOptions(setupAction);
    }
  }
}
