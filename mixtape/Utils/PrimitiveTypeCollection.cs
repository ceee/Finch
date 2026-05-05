using System.Collections.Concurrent;

namespace Mixtape.Utils;

public class PrimitiveTypeCollection : ConcurrentDictionary<Type, object>, IPrimitiveTypeCollection
{
  /// <summary>
  /// Initializes a new instance of <see cref="PrimitiveTypeCollection"/>.
  /// </summary>
  public PrimitiveTypeCollection() { }

  /// <inheritdoc />
  public TValue Get<TValue>() => (TValue)this[typeof(TValue)];

  /// <inheritdoc />
  public void Set<TValue>(TValue instance) => this[typeof(TValue)] = instance;

  /// <inheritdoc />
  public void Remove<TValue>() => TryRemove(typeof(TValue), out _);
}


/// <summary>
/// Represents a simple collection based on type.
/// </summary>
public interface IPrimitiveTypeCollection : ICollection<KeyValuePair<Type, object>>
{
  /// <summary>
  /// Retrieves the requested value from the collection.
  /// </summary>
  /// <typeparam name="TValue">The value key.</typeparam>
  /// <returns>The requested value, or null if it is not present.</returns>
  TValue Get<TValue>();

  /// <summary>
  /// Sets the given value in the collection.
  /// </summary>
  /// <typeparam name="TValue">The value key.</typeparam>
  /// <param name="instance">The value value.</param>
  void Set<TValue>(TValue instance);

  /// <summary>
  /// Removes the given value from the collection.
  /// </summary>
  /// <typeparam name="TValue">The value key.</typeparam>
  /// <param name="instance">The value value.</param>
  void Remove<TValue>();
}
