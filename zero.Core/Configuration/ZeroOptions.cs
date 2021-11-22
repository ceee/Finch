using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace zero.Configuration;

public class ZeroOptions : IZeroOptions
{
  protected IServiceProvider ServiceProvider { get; set; }

  protected ConcurrentDictionary<Type, object> OptionsCache { get; private set; } = new();


  public ZeroOptions(IServiceProvider serviceProvider)
  {
    Version = "0.0.1-alpha.1";
    ServiceProvider = serviceProvider;

    //SupportedLanguages = new string[2] { "en-US", "de-DE" };
    //DefaultLanguage = SupportedLanguages[0];
    //TokenExpiration = 60 * 3;
    //BackofficePath = "/zero";
    //ExcludedPaths = new() { };
    //Raven = new()
    //{
    //  CollectionPrefix = String.Empty
    //};
  }


  /// <inheritdoc />
  public TOptions For<TOptions>() where TOptions : class
  {
    Type type = typeof(TOptions);

    if (!OptionsCache.TryGetValue(type, out object value))
    {
      IOptions<TOptions> options = ServiceProvider.GetService<IOptions<TOptions>>();
      value = options.Value;
      OptionsCache.TryAdd(type, value);
    }

    return value as TOptions;
  }



  /// <inheritdoc />
  public bool Initialized => !String.IsNullOrEmpty(For<RavenOptions>()?.Database);

  /// <inheritdoc />
  public string Version { get; private set; }

  ///// <inheritdoc />
  //public string DefaultLanguage { get; set; }

  ///// <inheritdoc />
  //public string[] SupportedLanguages { get; private set; }

  ///// <inheritdoc />
  //public int TokenExpiration { get; set; }

  ///// <inheritdoc />
  //public RavenOptions Raven { get; set; }

  ///// <inheritdoc />
  //public ApplicationOptions Applications { get; set; }

  ///// <inheritdoc />
  //public SectionOptions Sections { get; private set; }

  ///// <inheritdoc />
  //public FeatureOptions Features { get; private set; }

  ///// <inheritdoc />
  //public PageOptions Pages { get; private set; }

  ///// <inheritdoc />
  //public ModuleOptions Modules { get; private set; }

  ///// <inheritdoc />
  //public PermissionOptions Permissions { get; private set; }

  ///// <inheritdoc />
  //public SettingsOptions Settings { get; private set; }

  ///// <inheritdoc />
  //public SpaceOptions Spaces { get; private set; }

  ///// <inheritdoc />
  //public IntegrationOptions Integrations { get; private set; }

  ///// <inheritdoc />
  //public BlueprintOptions Blueprints { get; private set; }
}


public interface IZeroOptions
{
  /// <summary>
  /// The currently active version
  /// This should not be set manually, as it is used for setup and migrations and incremented automatically
  /// </summary>
  string Version { get; }

  /// <summary>
  /// Whether this zero instance is initialized (setup is completed)
  /// </summary>
  bool Initialized { get; }

  /// <summary>
  /// Get typed options (proxy to IOptions<TOptions>)
  /// </summary>
  TOptions For<TOptions>() where TOptions : class;

  ///// <summary>
  ///// Default language ISO code
  ///// </summary>
  //string DefaultLanguage { get; set; }

    ///// <summary>
    ///// Language ISO codes which are supported by the zero backoffice
    ///// </summary>
    //string[] SupportedLanguages { get; }

    ///// <summary>
    ///// Expiration in minutes of a generated change token for an entity
    ///// </summary>
    //int TokenExpiration { get; set; }

    ///// <summary>
    ///// RavenDB configuration data
    ///// </summary>
    //RavenOptions Raven { get; set; }

    ///// <summary>
    ///// Application options
    ///// </summary>
    //ApplicationOptions Applications { get; set; }
}
