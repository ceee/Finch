using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Finch.Configuration;
using Finch.Models;
using Finch.Modules;

namespace Finch.Sqlite;

public static class FinchBuilderExtensions
{
  public static FinchBuilder AddSqlite(this FinchBuilder builder)
  {
    builder.AddModule<FinchSqliteModule>();
    return builder;
  }
}

internal class FinchSqliteModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IDbConnectionFactory>(CreateDbConnectionFactory);
    services.AddScoped<IDbConnection>(CreateDbConnection);
    services.AddScoped<IDbOperations, DbOperations>();
    services.AddScoped<StoreContext>();
    services.AddScoped<IEntityModifiedHandler, EmptyEntityModifiedHandler>();
    services.AddOptions<FlavorOptions>();
    services.AddOptions<SqliteOptions>().Bind(configuration.GetSection("Finch:Sqlite"));
    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }


  protected IDbConnectionFactory CreateDbConnectionFactory(IServiceProvider services)
  {
    IFinchOptions options = services.GetService<IFinchOptions>();
    SqliteOptions sqliteOptions = options.For<SqliteOptions>();
    return new OrmLiteConnectionFactory(sqliteOptions.ConnectionString, SqliteDialect.Provider);
  }


  protected IDbConnection CreateDbConnection(IServiceProvider services)
  {
    IDbConnectionFactory factory = services.GetService<IDbConnectionFactory>();
    IFinchOptions options = services.GetService<IFinchOptions>();
    SqliteOptions sqliteOptions = options.For<SqliteOptions>();
    IDbConnection db = factory.CreateDbConnection();
    db.Open();
    sqliteOptions.OnConnectionCreate?.Invoke(db);
    return db;
  }
}