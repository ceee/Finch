using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Options;

namespace zero.Core.Integrations
{
  public class IntegrationService : IIntegrationService
  {
    /// <inheritdoc />
    public IReadOnlyCollection<IntegrationType> RegisteredTypes { get; private set; }

    protected IZeroOptions Options { get; private set; }

    protected IZeroStore Store { get; private set; }


    public IntegrationService(IZeroOptions options, IZeroStore store)
    {
      Options = options;
      Store = store;
      RegisteredTypes = options.Integrations.GetAllItems();
    }


    /// <inheritdoc />
    public async Task<T> Get<T>() where T : IIntegration, new()
    {
      IntegrationType type = RegisteredTypes.FirstOrDefault(x => x.ContentType == typeof(T));
      return await GetIntegration<T>(type);
    }


    /// <inheritdoc />
    public async Task<T> Get<T>(string alias) where T : IIntegration, new()
    {
      IntegrationType type = RegisteredTypes.FirstOrDefault(x => x.ContentType == typeof(T) && x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
      return await GetIntegration<T>(type);
    }


    /// <inheritdoc />
    public async Task<bool> Any(string tag)
    {
      IEnumerable<IntegrationType> types = RegisteredTypes.Where(x => x.Tags.Contains(tag, StringComparer.InvariantCultureIgnoreCase));
      
      if (!types.Any())
      {
        return false;
      }

      //if (types.Any(x => x.IsAutoActivated))
      //{
      //  return true;
      //}

      string[] aliases = types.Select(x => x.Alias).ToArray();

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session.Query<IIntegration>().AnyAsync(x => x.TypeAlias.In(aliases) && x.IsActive);
    }


    /// <summary>
    /// Get integration data from database
    /// </summary>
    async Task<T> GetIntegration<T>(IntegrationType type) where T : IIntegration, new()
    {
      if (type == null)
      {
        return default;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      T integration = await session.Query<T>().FirstOrDefaultAsync(x => x.TypeAlias == type.Alias && x.IsActive);

      if (integration == null)// && type.IsAutoActivated)
      {
        return new T();
      }

      return integration;
    }
  }


  public interface IIntegrationService
  {
    /// <summary>
    /// Get an integration by type
    /// </summary>
    Task<T> Get<T>() where T : IIntegration, new();

    /// <summary>
    /// Get an integration by type and alias
    /// </summary>
    Task<T> Get<T>(string alias) where T : IIntegration, new();

    /// <summary>
    /// Checks if any integrations for a certain tag are activated
    /// </summary>
    Task<bool> Any(string tag);
  }
}
