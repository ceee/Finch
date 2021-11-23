using Microsoft.Extensions.DependencyInjection;

namespace zero.Stores;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroStores(this IServiceCollection services)
  {
    services.AddScoped<IStoreContext, StoreContext>();
    services.AddScoped<IStoreOperations, StoreOperations>();
    services.AddSingleton<IStoreCache, StoreCache>();

    services.AddScoped<ICountriesStore, CountriesStore>();
    services.AddScoped<ILanguagesStore, LanguagesStore>();
    services.AddScoped<IMailTemplatesStore, MailTemplatesStore>();

    return services;
  }
}