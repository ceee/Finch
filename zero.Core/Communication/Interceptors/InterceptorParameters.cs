namespace zero.Communication;

public class InterceptorParameters
{
  /// <summary>
  /// The current zero context
  /// </summary>
  public IZeroContext Context { get; set; }

  /// <summary>
  /// Raven document store
  /// </summary>
  public IZeroStore Store { get; set; }

  /// <summary>
  /// Access to other interceptor methods
  /// </summary>
  public IInterceptors Interceptors { get; internal set; }

  /// <summary>
  /// Access to operations
  /// </summary>
  public IStoreOperations Operations { get; internal set; }

  /// <summary>
  /// Parameters from the interceptor which ran on before the operation (only available for completed operations)
  /// </summary>
  public Dictionary<string, object> Properties { get; set; } = new();

  /// <summary>
  /// Get a typed property
  /// </summary>
  public TProp Property<TProp>(string key) => Properties.GetValueOrDefault<TProp>(key);

  /// <summary>
  /// Get a typed property
  /// </summary>
  public bool TryGetProperty<TProp>(string key, out TProp property) => Properties.TryGetValue(key, out property);

  /// <summary>
  /// Holds a reference to the previously existing model when an update happens (can be null)
  /// </summary>
  public object PreviousModel { get; set; }
}