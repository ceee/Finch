using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core.Options;

namespace zero.Web
{
  public static class ZeroServiceCollectionExtensions
  {
    public static ZeroBuilder AddZero(this IMvcBuilder builder, IConfiguration configuration)
    {
      return new ZeroBuilder(builder, configuration, null);
    }

    public static ZeroBuilder AddZero(this IMvcBuilder builder, IConfiguration configuration, Action<IZeroStartupOptions> setupAction)
    {
      return new ZeroBuilder(builder, configuration, setupAction);
    }
  }
}