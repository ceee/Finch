using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace zero.Configuration;

public class ZeroOptions : IZeroOptions
{
  /// <inheritdoc />
  public bool Initialized => !String.IsNullOrEmpty(For<RavenOptions>()?.Database);

  /// <inheritdoc />
  public string Version { get; set; }

  /// <inheritdoc />
  public string ZeroPath { get; set; }

  /// <inheritdoc />
  public TimeSpan TokenExpiration { get; set; }

  /// <inheritdoc />
  public List<ApplicationRegistration> Applications { get; set; } = new();


  internal IServiceProvider ServiceProvider { get; set; }

  protected ConcurrentDictionary<Type, object> OptionsCache { get; private set; } = new();


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
}


public interface IZeroOptions
{
  /// <summary>
  /// Path to the backoffice. Defaults to /zero
  /// </summary>
  string ZeroPath { get; set; }

  /// <summary>
  /// The currently active version
  /// This should not be set manually, as it is used for setup and migrations and incremented automatically
  /// </summary>
  string Version { get; set; }

  /// <summary>
  /// Whether this zero instance is initialized (setup is completed)
  /// </summary>
  bool Initialized { get; }

  /// <summary>
  /// Expiration of a generated change token for an entity
  /// </summary>
  TimeSpan TokenExpiration { get; set; }

  /// <summary>
  /// Contains all registered applications
  /// </summary>
  List<ApplicationRegistration> Applications { get; set; }

  /// <summary>
  /// Get typed options (proxy to IOptions<TOptions>)
  /// </summary>
  TOptions For<TOptions>() where TOptions : class;
}
