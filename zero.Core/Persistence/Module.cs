using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Http;

namespace zero.Persistence;

internal class PersistenceModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddSingleton<IZeroDocumentConventionsBuilder, ZeroDocumentConventionsBuilder>();
    config.Services.AddSingleton<IZeroDocumentStore, ZeroDocumentStore>(CreateRavenStore);
    config.Services.AddScoped<IZeroStore, ZeroStore>();
    config.Services.AddScoped<IZeroTokenProvider, ZeroTokenProvider>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    base.Configure(options);
  }


  /// <summary>
  /// Creates and configures the raven store
  /// </summary>
  protected ZeroDocumentStore CreateRavenStore(IServiceProvider services)
  {
    IZeroOptions options = services.GetService<IZeroOptions>();
    IZeroDocumentConventionsBuilder conventionsBuilder = services.GetService<IZeroDocumentConventionsBuilder>();

    IZeroDocumentStore store = new ZeroDocumentStore(options)
    {
      Urls = new string[1] { options.Raven.Url },
      Conventions = // TODO activate and test this
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
    if (options.SetupCompleted)
    {
      //var indexes = options.Raven.Indexes.BuildAll(options, store);
      //IndexCreation.CreateIndexes(indexes, store, database: options.Raven.Database);
    }

    return (ZeroDocumentStore)raven;
  }
}