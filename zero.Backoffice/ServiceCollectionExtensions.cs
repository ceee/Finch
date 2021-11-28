using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice;

public static class ServiceCollectionExtensions
{
  public static ZeroBuilder AddBackoffice<T>(this ZeroBuilder builder) where T : ZeroBackofficePlugin, IZeroPlugin, new()
  {
    return builder.AddBackoffice<T>(_ => { });
  }

  public static ZeroBuilder AddBackoffice(this ZeroBuilder builder)
  {
    return builder.AddBackoffice<ZeroBackofficePlugin>();
  }

  public static ZeroBuilder AddBackoffice<T>(this ZeroBuilder builder, Action<BackofficeOptions> options) where T : ZeroBackofficePlugin, IZeroPlugin, new()
  {
    return builder.AddPlugin(services =>
    {
      T plugin = new();
      plugin.PostConfigureServices = (services, configuration) =>
      {
        services.Configure<BackofficeOptions>(opts => options(opts));
      };

      return plugin;
    });
  }
}