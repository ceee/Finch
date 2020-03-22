namespace unjo.Core
{
  public class BackofficeConfiguration : IBackofficeConfiguration
  {
    public string SystemUserId { get; set; }

    public FoldersConfig Folders { get; set; }
    
    public RavenConfig Raven { get; set; }

    public LoggingConfig Logging { get; set; }
  }

  public interface IBackofficeConfiguration
  {
    string SystemUserId { get; set; }

    FoldersConfig Folders { get; set; }
   
    RavenConfig Raven { get; set; }

    LoggingConfig Logging { get; set; }
  }

  public class FoldersConfig
  {
    public string Temp { get; set; }
  }

  public class LoggingConfig
  {
    public bool IncludeScopes { get; set; }
    public LogLevel LogLevel { get; set; }
  }

  public class LogLevel
  {
    public string Default { get; set; }
    public string System { get; set; }
    public string Microsoft { get; set; }
  }

  public class RavenConfig
  {
    public string Url { get; set; }
    public string Database { get; set; }
    public string License { get; set; }
  }
}
