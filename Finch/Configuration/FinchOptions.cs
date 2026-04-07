using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Finch.Configuration;

public class FinchOptions : IFinchOptions
{
  /// <inheritdoc />
  public string Version { get; set; }

  /// <inheritdoc />
  public string Language { get; set; } = "en_US";

  /// <inheritdoc />
  public string AppName { get; set; }

  /// <inheritdoc />
  public TimeSpan TokenExpiration { get; set; }


  internal IServiceProvider ServiceProvider { get; set; }

  protected ConcurrentDictionary<Type, object> OptionsCache { get; private set; } = new();


  /// <inheritdoc />
  public TOptions For<TOptions>() where TOptions : class
  {
    Type type = typeof(TOptions);

    if (!OptionsCache.TryGetValue(type, out object value) && ServiceProvider != null)
    {
      IOptions<TOptions> options = ServiceProvider.GetService<IOptions<TOptions>>();
      value = options.Value;
      OptionsCache.TryAdd(type, value);
    }

    return value as TOptions;
  }
}


public interface IFinchOptions
{
  /// <summary>
  /// The currently active version
  /// This should not be set manually, as it is used for setup and migrations and incremented automatically
  /// </summary>
  string Version { get; set; }

  /// <summary>
  /// ISO Code for language
  /// </summary>
  public string Language { get; set; }

  /// <summary>
  /// Name of the app (used in logging and otehr areas)
  /// </summary>
  public string AppName { get; set; }

  /// <summary>
  /// Expiration of a generated change token for an entity
  /// </summary>
  TimeSpan TokenExpiration { get; set; }

  /// <summary>
  /// Get typed options (proxy to IOptions<TOptions>)
  /// </summary>
  TOptions For<TOptions>() where TOptions : class;
}
