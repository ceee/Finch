using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Configuration;

internal class ConfigurationModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddOptions<ZeroOptions>()
      .Bind(config.Configuration.GetSection("Zero"))
      .Configure(opts => opts.ZeroVersion = "0.0.1.0" /*// TODO*/);

    config.Services.AddTransient<IZeroOptions>(factory => factory.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue);
  }
}