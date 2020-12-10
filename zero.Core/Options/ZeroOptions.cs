using System;
using System.Collections.Generic;

namespace zero.Core.Options
{
  public class ZeroOptions : IZeroOptions
  {
    public ZeroOptions()
    {
      SupportedLanguages = new string[2] { "en-US", "de-DE" };
      DefaultLanguage = SupportedLanguages[0];
      TokenExpiration = 60;
      BackofficePath = "/zero";
      ExcludedPaths = new List<string>() { };
      Raven = new RavenOptions()
      {
        CollectionPrefix = String.Empty
      };
      Sections = new SectionOptions();
      Features = new FeatureOptions();
      Pages = new PageOptions();
      Modules = new ModuleOptions();
      Permissions = new PermissionOptions();
      Settings = new SettingsOptions();
      Spaces = new SpaceOptions();
      Integrations = new IntegrationOptions();
    }

    /// <inheritdoc />
    public bool SetupCompleted => !String.IsNullOrEmpty(Raven?.Database);

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

    /// <summary>
    /// Paths in the backoffice which are not handled by zero
    /// </summary>
    public List<string> ExcludedPaths { get; private set; }

    /// <inheritdoc />
    //public IZeroPluginConfiguration Backoffice { get; set; }

    /// <inheritdoc />
    public SectionOptions Sections { get; private set; }

    /// <inheritdoc />
    public FeatureOptions Features { get; private set; }

    /// <inheritdoc />
    public PageOptions Pages { get; private set; }

    /// <inheritdoc />
    public ModuleOptions Modules { get; private set; }

    /// <inheritdoc />
    public PermissionOptions Permissions { get; private set; }

    /// <inheritdoc />
    public SettingsOptions Settings { get; private set; }

    /// <inheritdoc />
    public SpaceOptions Spaces { get; private set; }

    /// <inheritdoc />
    public IntegrationOptions Integrations { get; private set; }
  }


  public class RavenOptions
  {
    public string Url { get; set; }

    public string Database { get; set; }

    public string CollectionPrefix { get; set; }
  }


  public interface IZeroOptions
  {
    /// <summary>
    /// Whether the zero setup has already been completed and the instance is ready for use
    /// </summary>
    bool SetupCompleted { get; }

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
    /// Paths in the backoffice which are not handled by zero
    /// </summary>
    List<string> ExcludedPaths { get; }

    /// <summary>
    /// 
    /// </summary>
    SectionOptions Sections { get; }

    /// <summary>
    /// 
    /// </summary>
    FeatureOptions Features { get; }

    /// <summary>
    /// 
    /// </summary>
    PageOptions Pages { get; }

    /// <summary>
    /// 
    /// </summary>
    ModuleOptions Modules { get; }

    /// <summary>
    /// 
    /// </summary>
    PermissionOptions Permissions { get; }

    /// <summary>
    /// 
    /// </summary>
    SettingsOptions Settings { get; }

    /// <summary>
    /// 
    /// </summary>
    SpaceOptions Spaces { get; }

    /// <summary>
    /// 
    /// </summary>
    IntegrationOptions Integrations { get; }
  }
}
