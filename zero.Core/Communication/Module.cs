using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal class CommunicationModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IInterceptors, Interceptors>();
    config.Services.AddSingleton<IMessageAggregator, MessageAggregator>();
    config.Services.AddTransient<IHandlerHolder, HandlerHolder>();
  }
}