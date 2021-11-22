using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Configuration;

internal class ConfigurationModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddOptions<ZeroOptions>().Bind(config.Configuration.GetSection("Zero")).Configure(opts => { });
    config.Services.AddTransient<IZeroOptions>(factory => factory.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue);
  }
}