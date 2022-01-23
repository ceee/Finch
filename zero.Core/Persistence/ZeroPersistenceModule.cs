using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Http;

namespace zero.Persistence;

internal class ZeroPersistenceModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IZeroDocumentConventionsBuilder, ZeroDocumentConventionsBuilder>();
    services.AddSingleton<IZeroDocumentStore, ZeroDocumentStore>(CreateRavenStore);
    services.AddScoped<IZeroStore, ZeroStore>();
    services.AddScoped<IZeroTokenProvider, ZeroTokenProvider>();
    services.AddScoped<IRevisionService, RevisionService>();

    services.AddOptions<RavenOptions>().Bind(configuration.GetSection("Zero:Raven"));
  }

  /// <summary>
  /// Creates and configures the raven store
  /// </summary>
  protected ZeroDocumentStore CreateRavenStore(IServiceProvider services)
  {
    IZeroOptions options = services.GetService<IZeroOptions>();
    RavenOptions ravenOptions = options.For<RavenOptions>();
    IZeroDocumentConventionsBuilder conventionsBuilder = services.GetService<IZeroDocumentConventionsBuilder>();

    IZeroDocumentStore store = new ZeroDocumentStore(options)
    {
      Urls = new string[1] { ravenOptions.Url },
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
      var indexes = ravenOptions.Indexes.BuildAll(options, store);
      IndexCreation.CreateIndexes(indexes, store, database: ravenOptions.Database);
    }

    return (ZeroDocumentStore)raven;
  }
}