using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Communication;

public class CommunicationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IInterceptors, Interceptors>();
    services.AddSingleton<IMessageAggregator, MessageAggregator>();
    services.AddTransient<IHandlerHolder, HandlerHolder>();
  }
}