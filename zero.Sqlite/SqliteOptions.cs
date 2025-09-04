using System;
using System.Data;

namespace zero.Sqlite;

public class SqliteOptions
{
  public string ConnectionString { get; set; }

  public Action<IDbConnection> OnConnectionCreate { get; set; }
}