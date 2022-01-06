using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Communication;

internal class ZeroCommunicationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IInterceptors, Interceptors>();
    services.AddSingleton<IMessageAggregator, MessageAggregator>();
    services.AddTransient<IHandlerHolder, HandlerHolder>();
    services.AddTransient(typeof(Lazy<>), typeof(LazilyResolved<>));
  }
}