using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Http;
using Finch.Identity;
using Finch.Media;
using Finch.Numbers;

namespace Finch.Raven;

public static class FinchBuilderExtensions
{
  public static FinchBuilder AddRavenDb(this FinchBuilder builder)
  {
    builder.AddModule<FinchRavenModule>();
    return builder;
  }
}

internal class FinchRavenModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IRavenDocumentConventionsBuilder, RavenDocumentConventionsBuilder>();
    services.AddSingleton<IDocumentStore>(CreateRavenStore);
    services.AddScoped<IFinchStore, FinchStore>();
    services.AddScoped<IFinchTokenProvider, FinchTokenProvider>();
    services.AddScoped<StoreContext>();
    services.AddTransient<IRavenOperations, RavenOperations>();
    services.AddScoped<IInterceptors, Interceptors>();

    services.Replace<IFinchIdentityStoreDbProvider, RavenIdentityStoreDbProvider>(ServiceLifetime.Scoped);
    services.Replace<IFinchMediaStoreDbProvider, RavenMediaStoreDbProvider>(ServiceLifetime.Scoped);
    services.Replace<IFinchNumberStoreDbProvider, RavenNumberStoreDbProvider>(ServiceLifetime.Scoped);

    services.AddOptions<FlavorOptions>();
    services.AddOptions<RavenOptions>().Bind(configuration.GetSection("Finch:Raven"));
    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }

  /// <summary>
  /// Creates and configures the raven store
  /// </summary>
  protected IDocumentStore CreateRavenStore(IServiceProvider services)
  {
    IFinchOptions options = services.GetService<IFinchOptions>();
    RavenOptions ravenOptions = options.For<RavenOptions>();
    IRavenDocumentConventionsBuilder conventionsBuilder = services.GetService<IRavenDocumentConventionsBuilder>();

    IDocumentStore store = new DocumentStore()
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
    IEnumerable<IFinchIndexDefinition> indexes = ravenOptions.Indexes.BuildAll(options, store); 
    IndexCreation.CreateIndexes(indexes, store, database: ravenOptions.Database);

      
    return raven;
  }
}