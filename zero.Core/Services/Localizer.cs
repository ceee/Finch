using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using zero.Core.Attributes;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Renderer;

namespace zero.Core.Services
{
  public class Localizer : ILocalizer, IDisposable
  {
    protected Dictionary<string, string> Cache { get; private set; } = new();

    protected IZeroStore Store { get; private set; }

    IDocumentSession _session = null;
    protected IDocumentSession Session { get { return _session ?? (_session = Store.OpenSession()); } }


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

        lock (Cache) // TOOD use concurrent dictionary
        {
          Cache[key] = value;
        }
      }

      if (tokens != null)
      {
        value = TokenReplacement.Apply(value, tokens);
      }

      return value;
    }


    /// <inheritdoc />
    public string Text<T>(T enumValue) where T : Enum
    {
      Type type = enumValue.GetType();
      MemberInfo memInfo = type.GetMember(enumValue.ToString())[0];
      return Text(memInfo.GetCustomAttribute<LocalizeAttribute>()?.Key);
    }


    /// <inheritdoc />
    public string Maybe(string key) => Maybe(key, null);


    /// <inheritdoc />
    public string Maybe(string key, Dictionary<string, string> tokens)
    {
      return key.IsNullOrEmpty() || !key.StartsWith("@") ? key : Text(key.Substring(1), tokens);
    }


    /// <inheritdoc />
    public void Dispose()
    {
      Session?.Dispose();
    }


    /// <summary>
    /// Get translation from database or any other source
    /// </summary>
    protected virtual Translation LoadTranslation(string key)
    {
      return Session.Query<Translation>().FirstOrDefault(x => x.Key == key);
    }
  }

  public interface ILocalizer : IDisposable
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
    /// Only tries to resolve the key when it is prefixed with an @
    /// </summary>
    string Maybe(string key);

    /// <summary>
    /// Only tries to resolve the key when it is prefixed with an @
    /// </summary>
    string Maybe(string key, Dictionary<string, string> tokens);
  }
}
