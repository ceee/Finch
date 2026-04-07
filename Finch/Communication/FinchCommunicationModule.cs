using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Communication;

internal class FinchCommunicationModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IMessageAggregator, MessageAggregator>();
    services.AddTransient<IHandlerHolder, HandlerHolder>();
    services.AddTransient(typeof(Lazy<>), typeof(LazilyResolved<>));
  }
}