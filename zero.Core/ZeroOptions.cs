using zero.Core.Plugins;

namespace zero.Core
{
  public class ZeroOptions : IZeroOptions
  {
    public ZeroOptions()
    {
      SupportedLanguages = new string[2] { "en-US", "de-DE" };
      DefaultLanguage = SupportedLanguages[0];
      TokenExpiration = 60;
      BackofficePath = "/zero";
    }

    /// <inheritdoc />
    public string ZeroVersion { get; set; }

    /// <inheritdoc />
    public string DefaultLanguage { get; set; }

    /// <inheritdoc />
    public string[] SupportedLanguages { get; private set; }

    /// <inheritdoc />
    public int TokenExpiration { get; set; }

    /// <inheritdoc />
    public RavenOptions Raven { get; set; }

    /// <inheritdoc />
    public string BackofficePath { get; set; }

    /// <inheritdoc />
    public ZeroPlugin Backoffice { get; set; }
  }


  public class RavenOptions
  {
    public string Url { get; set; }

    public string Database { get; set; }
  }


  public interface IZeroOptions
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
    /// Language ISO codes which are supported by the zero backoffice
    /// </summary>
    string[] SupportedLanguages { get; }

    /// <summary>
    /// Expiration in minutes of a generated change token for an entity
    /// </summary>
    int TokenExpiration { get; set; }

    /// <summary>
    /// RavenDB configuration data
    /// </summary>
    RavenOptions Raven { get; set; }

    /// <summary>
    /// URL path to the backoffice (defaults to /zero)
    /// </summary>
    string BackofficePath { get; set; }

    /// <summary>
    /// Default settings for the backoffice
    /// </summary>
    ZeroPlugin Backoffice { get; set; }
  }
}
