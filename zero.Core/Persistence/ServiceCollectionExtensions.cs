using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Http;

namespace zero.Persistence;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroPersistence(this IServiceCollection services)
  {
    services.AddSingleton<IZeroDocumentConventionsBuilder, ZeroDocumentConventionsBuilder>();
    services.AddSingleton<IZeroDocumentStore, ZeroDocumentStore>(CreateRavenStore);
    services.AddScoped<IZeroStore, ZeroStore>();
    services.AddScoped<IZeroTokenProvider, ZeroTokenProvider>();
    return services;
  }


  /// <summary>
  /// Creates and configures the raven store
  /// </summary>
  static ZeroDocumentStore CreateRavenStore(IServiceProvider services)
  {
    IZeroOptions options = services.GetService<IZeroOptions>();
    IZeroDocumentConventionsBuilder conventionsBuilder = services.GetService<IZeroDocumentConventionsBuilder>();

    IZeroDocumentStore store = new ZeroDocumentStore(options)
    {
      Urls = new string[1] { options.For<RavenOptions>().Url },
      Conventions =
      {
        AggressiveCache =
        {
          Duration = TimeSpan.FromHours(1),
          Mode = AggressiveCacheMode.TrackChanges
        }
      }
    };

    conventionsBuilder.Run(store.Conventions);

    IDocumentStore raven = store.Initialize();

    // create all indexes
    if (options.Initialized)
    {
      //var indexes = options.Raven.Indexes.BuildAll(options, store);
      //IndexCreation.CreateIndexes(indexes, store, database: options.Raven.Database);
    }

    return (ZeroDocumentStore)raven;
  }
}