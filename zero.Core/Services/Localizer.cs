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
    public string Text(string key, IDictionary<string, string> tokens)
    {
      if (key.IsNullOrEmpty())
      {
        return null;
      }

      if (!Cache.TryGetValue(key, out string value))
      {
        ITranslation translation = LoadTranslation(key);

        if (translation == null)
        {
          return null;
        }

        value = translation.Value;

        lock (Cache)
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


    /// <summary>
    /// Get a text string from a Text attribute on a specific enum value
    /// </summary>
    public string Text<T>(T enumValue) where T : Enum
    {
      Type type = enumValue.GetType();
      MemberInfo memInfo = type.GetMember(enumValue.ToString())[0];
      return Text(memInfo.GetCustomAttribute<LocalizeAttribute>()?.Key);
    }


    /// <inheritdoc />
    public void Dispose()
    {
      Session?.Dispose();
    }


    /// <summary>
    /// Get translation from database or any other source
    /// </summary>
    protected virtual ITranslation LoadTranslation(string key)
    {
      return Session.Query<ITranslation>().FirstOrDefault(x => x.Key == key);
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
    string Text(string key, IDictionary<string, string> tokens);

    /// <summary>
    /// Get a text string from a [Localize] attribute
    /// </summary>
    public string Text<T>(T enumValue) where T : Enum;
  }
}
