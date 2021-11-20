using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal class CommunicationModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddSingleton<IMessageAggregator, MessageAggregator>();
  }
}