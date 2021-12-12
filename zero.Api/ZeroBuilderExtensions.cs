using Microsoft.Extensions.DependencyInjection;

namespace zero.Api;

public static class ZeroBuilderExtensions
{
  public static ZeroBuilder AddApi<T>(this ZeroBuilder builder) where T : ZeroApiPlugin, IZeroPlugin, new()
  {
    return builder.AddApi<T>(_ => { });
  }

  public static ZeroBuilder AddApi(this ZeroBuilder builder)
  {
    return builder.AddApi<ZeroApiPlugin>();
  }

  public static ZeroBuilder AddApi<T>(this ZeroBuilder builder, Action<ApiOptions> options) where T : ZeroApiPlugin, IZeroPlugin, new()
  {
    return builder.AddPlugin(services =>
    {
      T plugin = new();
      plugin.PostConfigureServices = (services, configuration) =>
      {
        services.Configure<ApiOptions>(opts => options(opts));
      };

      return plugin;
    });
  }
}