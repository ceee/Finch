namespace zero.Core
{
  public class ZeroConfiguration : IZeroConfiguration
  {
    /// <inheritdoc />
    public string ZeroVersion { get; set; }

    /// <inheritdoc />
    public string DefaultLanguage { get; set; }

    /// <inheritdoc />
    public RavenConfig Raven { get; set; }

    /// <inheritdoc />
    public LoggingConfig Logging { get; set; }
  }

  public interface IZeroConfiguration
  {
    /// <summary>
    /// The currently active version
    /// This should not be set manually, as it is used for setup and migrations and incremented automatically
    /// </summary>
    string ZeroVersion { get; set; }

    /// <summary>
    /// Default language ISO code
    /// </summary>
    string DefaultLanguage { get; set; }

    /// <summary>
    /// RavenDB configuration data
    /// </summary>
    RavenConfig Raven { get; set; }

    /// <summary>
    /// Logging
    /// </summary>
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
  }
}
