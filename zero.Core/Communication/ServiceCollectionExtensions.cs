using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Communication;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroCommunication(this IServiceCollection services)
  {
    services.AddScoped<IInterceptors, Interceptors>();
    services.AddSingleton<IMessageAggregator, MessageAggregator>();
    services.AddTransient<IHandlerHolder, HandlerHolder>();
    return services;
  }
}