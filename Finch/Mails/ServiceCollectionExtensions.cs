using Finch.Mails.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Mails;

public static class MailsServiceCollectionExtensions
{
  /// <summary>
  /// Replaces the current mail dispatcher implementation with a new one
  /// </summary>
  public static IServiceCollection AddMailDispatcher<T>(this IServiceCollection services) where T : class, IMailDispatcher
  {
    services.Replace<IMailDispatcher, T>(ServiceLifetime.Scoped);
    return services;
  }
}