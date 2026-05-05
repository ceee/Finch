using System;
using System.Data;

namespace Mixtape.Sqlite;

public class SqliteOptions
{
  public string ConnectionString { get; set; }

  public Action<IDbConnection> OnConnectionCreate { get; set; }
}