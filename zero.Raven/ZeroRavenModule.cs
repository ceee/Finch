using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Http;
using zero.Identity;
using zero.Media;
using zero.Numbers;

namespace zero.Raven;

public static class ZeroBuilderExtensions
{
  public static ZeroBuilder AddRavenDb(this ZeroBuilder builder)
  {
    builder.AddModule<ZeroRavenModule>();
    return builder;
  }
}

internal class ZeroRavenModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IRavenDocumentConventionsBuilder, RavenDocumentConventionsBuilder>();
    services.AddSingleton<IZeroDocumentStore, ZeroDocumentStore>(CreateRavenStore);
    services.AddScoped<IZeroStore, ZeroStore>();
    services.AddScoped<IZeroTokenProvider, ZeroTokenProvider>();
    services.AddScoped<StoreContext>();
    services.AddTransient<IRavenOperations, RavenOperations>();
    services.AddScoped<IInterceptors, Interceptors>();

    services.AddScoped<IZeroIdentityStoreDbProvider, RavenIdentityStoreDbProvider>();
    services.AddScoped<IZeroMediaStoreDbProvider, RavenMediaStoreDbProvider>();
    services.AddScoped<IZeroNumberStoreDbProvider, RavenNumberStoreDbProvider>();

    services.AddOptions<FlavorOptions>();
    services.AddOptions<RavenOptions>().Bind(configuration.GetSection("Zero:Raven"));
    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }

  /// <summary>
  /// Creates and configures the raven store
  /// </summary>
  protected ZeroDocumentStore CreateRavenStore(IServiceProvider services)
  {
    IZeroOptions options = services.GetService<IZeroOptions>();
    RavenOptions ravenOptions = options.For<RavenOptions>();
    IRavenDocumentConventionsBuilder conventionsBuilder = services.GetService<IRavenDocumentConventionsBuilder>();

    IZeroDocumentStore store = new ZeroDocumentStore(options)
    {
      Database = ravenOptions.Database,
      Urls = new string[1] { ravenOptions.Url },
      Conventions =
      {
        AggressiveCache =
        {
          Duration = TimeSpan.FromMinutes(ravenOptions.CacheInMinutes),
          Mode = AggressiveCacheMode.TrackChanges
        }
      }
    };

    conventionsBuilder.Run(store.Conventions);

    IDocumentStore raven = store.Initialize();

    // create all indexes
    IEnumerable<IZeroIndexDefinition> indexes = ravenOptions.Indexes.BuildAll(options, store); 
    IndexCreation.CreateIndexes(indexes, store, database: ravenOptions.Database);

      
    return (ZeroDocumentStore)raven;
  }
}