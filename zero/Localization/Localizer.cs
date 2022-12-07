using System.Collections.Concurrent;
using System.Reflection;

namespace zero.Localization;

public class Localizer : ILocalizer
{
  protected ConcurrentDictionary<string, string> Cache { get; private set; } = new();

  protected IZeroStore Store { get; private set; }

  public Localizer(IZeroStore store)
  {
    Store = store;
  }


  /// <inheritdoc />
  public string Text(string key) => Text(key, null);


  /// <inheritdoc />
  public string Text(string key, Dictionary<string, string> tokens)
  {
    if (key.IsNullOrEmpty())
    {
      return null;
    }

    if (!Cache.TryGetValue(key, out string value))
    {
      Translation translation = LoadTranslation(key);

      if (translation == null)
      {
        return null;
      }

      value = translation.Value;
      Cache.TryAdd(key, value);
    }

    if (tokens != null)
    {
      value = TokenReplacement.Apply(value, tokens);
    }

    return value;
  }


  /// <inheritdoc />
  public string Text<T>(T enumValue) where T : Enum => Text(enumValue, null);


  /// <inheritdoc />
  public string Text<T>(T enumValue, Dictionary<string, string> tokens) where T : Enum
  {
    Type type = enumValue.GetType();
    MemberInfo memInfo = type.GetMember(enumValue.ToString())[0];
    return Text(memInfo.GetCustomAttribute<LocalizeAttribute>()?.Key, tokens);
  }


  /// <inheritdoc />
  public string Maybe(string key) => Maybe(key, null);


  /// <inheritdoc />
  public string Maybe(string key, Dictionary<string, string> tokens)
  {
    return key.IsNullOrEmpty() || !key.StartsWith("@") ? key : Text(key.Substring(1), tokens);
  }


  /// <summary>
  /// Get translation from database or any other source
  /// </summary>
  protected virtual Translation LoadTranslation(string key)
  {
    return Store.Session().Synchronous.Query<Translation>().FirstOrDefault(x => x.Key == key);
  }
}

public interface ILocalizer
{   
  /// <summary>
  /// 
  /// </summary>
  string Text(string key);

  /// <summary>
  /// 
  /// </summary>
  string Text(string key, Dictionary<string, string> tokens);

  /// <summary>
  /// Get a text string from a [Localize] attribute
  /// </summary>
  string Text<T>(T enumValue) where T : Enum;

  /// <summary>
  /// Get a text string from a [Localize] attribute
  /// </summary>
  string Text<T>(T enumValue, Dictionary<string, string> tokens) where T : Enum;

  /// <summary>
  /// Only tries to resolve the key when it is prefixed with an @
  /// </summary>
  string Maybe(string key);

  /// <summary>
  /// Only tries to resolve the key when it is prefixed with an @
  /// </summary>
  string Maybe(string key, Dictionary<string, string> tokens);
}
