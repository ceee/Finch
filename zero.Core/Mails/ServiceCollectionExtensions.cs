using Microsoft.Extensions.DependencyInjection;

namespace zero.Mails;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroMails(this IServiceCollection services)
  {
    services.AddScoped<IMailProvider, MailProvider>();
    services.AddScoped<IMailDispatcher, LoggerMailDispatcher>();
    services.AddScoped<IMailTemplatesStore, MailTemplatesStore>();
    return services;
  }
}