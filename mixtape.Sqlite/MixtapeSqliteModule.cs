using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Mixtape.Configuration;
using Mixtape.Models;
using Mixtape.Modules;

namespace Mixtape.Sqlite;

public static class MixtapeBuilderExtensions
{
  public static MixtapeBuilder AddSqlite(this MixtapeBuilder builder)
  {
    builder.AddModule<MixtapeSqliteModule>();
    return builder;
  }
}

internal class MixtapeSqliteModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IDbConnectionFactory>(CreateDbConnectionFactory);
    services.AddScoped<IDbConnection>(CreateDbConnection);
    services.AddScoped<IDbOperations, DbOperations>();
    services.AddScoped<StoreContext>();
    services.AddScoped<IEntityModifiedHandler, EmptyEntityModifiedHandler>();
    services.AddOptions<FlavorOptions>();
    services.AddOptions<SqliteOptions>().Bind(configuration.GetSection("Mixtape:Sqlite"));
    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }


  protected IDbConnectionFactory CreateDbConnectionFactory(IServiceProvider services)
  {
    IMixtapeOptions options = services.GetService<IMixtapeOptions>();
    SqliteOptions sqliteOptions = options.For<SqliteOptions>();
    return new OrmLiteConnectionFactory(sqliteOptions.ConnectionString, SqliteDialect.Provider);
  }


  protected IDbConnection CreateDbConnection(IServiceProvider services)
  {
    IDbConnectionFactory factory = services.GetService<IDbConnectionFactory>();
    IMixtapeOptions options = services.GetService<IMixtapeOptions>();
    SqliteOptions sqliteOptions = options.For<SqliteOptions>();
    IDbConnection db = factory.CreateDbConnection();
    db.Open();
    sqliteOptions.OnConnectionCreate?.Invoke(db);
    return db;
  }
}