using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using zero.Configuration;
using zero.Models;
using zero.Modules;

namespace zero.Sqlite;

public static class ZeroBuilderExtensions
{
  public static ZeroBuilder AddSqlite(this ZeroBuilder builder)
  {
    builder.AddModule<ZeroSqliteModule>();
    return builder;
  }
}

internal class ZeroSqliteModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IDbConnectionFactory>(CreateDbConnectionFactory);
    services.AddScoped<IDbConnection>(CreateDbConnection);
    services.AddScoped<IDbOperations, DbOperations>();
    services.AddScoped<StoreContext>();
    services.AddOptions<FlavorOptions>();
    services.AddOptions<SqliteOptions>().Bind(configuration.GetSection("Zero:Sqlite"));
    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }


  protected IDbConnectionFactory CreateDbConnectionFactory(IServiceProvider services)
  {
    IZeroOptions options = services.GetService<IZeroOptions>();
    SqliteOptions ravenOptions = options.For<SqliteOptions>();
    return new OrmLiteConnectionFactory(ravenOptions.ConnectionString, SqliteDialect.Provider);
  }


  protected IDbConnection CreateDbConnection(IServiceProvider services)
  {
    IDbConnectionFactory factory = services.GetService<IDbConnectionFactory>();
    IDbConnection db = factory.CreateDbConnection();
    db.Open();
    //db.CreateTableIfNotExists<News>();
    return db;
  }
}