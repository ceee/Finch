using System;
using System.Collections.Generic;

namespace zero;

public class ZeroOptions : IZeroOptions
{
  public ZeroOptions()
  {
    SupportedLanguages = new string[2] { "en-US", "de-DE" };
    DefaultLanguage = SupportedLanguages[0];
    TokenExpiration = 60 * 3;
    BackofficePath = "/zero";
    ExcludedPaths = new() { };
    Raven = new()
    {
      CollectionPrefix = String.Empty
    };
    Sections = new();
    Features = new();
    Pages = new();
    Modules = new();
    Permissions = new();
    Settings = new();
    Spaces = new();
    Integrations = new();
    Icons = new();
    Services = new();
    Blueprints = new();
    Interceptors = new();
    Applications = new();

    //Raven.Indexes.Add<Backoffice_Search>();
    //Raven.Indexes.Add<Media_ByChildren>();
    //Raven.Indexes.Add<Media_ByParent>();
    //Raven.Indexes.Add<MediaFolder_ByHierarchy>();
    //Raven.Indexes.Add<MediaFolders_WithChildren>();
    //Raven.Indexes.Add<Pages_AsHistory>();
    //Raven.Indexes.Add<Pages_ByHierarchy>();
    //Raven.Indexes.Add<Pages_WithChildren>();
    //Raven.Indexes.Add<Pages_ByType>();
    //Raven.Indexes.Add<Routes_ForResolver>();
    //Raven.Indexes.Add<Routes_ByDependencies>();

    //Raven.Indexes.Add<zero_Countries>();
    //Raven.Indexes.Add<zero_Languages>();
    //Raven.Indexes.Add<zero_Translations>();
    //Raven.Indexes.Add<zero_MailTemplates>();
    //Raven.Indexes.Add<zero_Spaces>();
    //Raven.Indexes.Add<zero_RecycledEntities>();

    Search = new();
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
  public ApplicationOptions Applications { get; set; }

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

  /// <inheritdoc />
  public IconOptions Icons { get; private set; }

  /// <inheritdoc />
  public ServiceOptions Services { get; private set; }

  /// <inheritdoc />
  public BlueprintOptions Blueprints { get; private set; }

  /// <inheritdoc />
  public InterceptorOptions Interceptors { get; private set; }

  /// <inheritdoc />
  public SearchOptions Search { get; private set; }
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
  /// Application options
  /// </summary>
  ApplicationOptions Applications { get; set; }

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

  /// <summary>
  /// 
  /// </summary>
  IconOptions Icons { get; }

  /// <summary>
  /// 
  /// </summary>
  ServiceOptions Services { get; }

  /// <summary>
  /// 
  /// </summary>
  BlueprintOptions Blueprints { get; }

  /// <summary>
  /// 
  /// </summary>
  InterceptorOptions Interceptors { get; }

  /// <summary>
  /// 
  /// </summary>
  SearchOptions Search { get; }
}
