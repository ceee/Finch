using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;

namespace zero.Core.Integrations
{
  public class IntegrationsCollection : CollectionCacheBase<IIntegration>, IIntegrationsCollection
  {
    public IReadOnlyCollection<IIntegrationType> RegisteredTypes { get; private set; }


    public IntegrationsCollection(IZeroContext context, ICollectionInterceptorHandler interceptorHandler) : base(context, interceptorHandler)
    {
      RegisteredTypes = context.Options.Integrations.GetAllItems();
    }


    /// <inheritdoc />
    public async Task<T> Get<T>() where T : class, IIntegration
    {
      Type type = typeof(T);
      IIntegrationType integration = RegisteredTypes.FirstOrDefault(x => x.SettingsType == type);

      if (integration == null)
      {
        return default;
      }

      await Preload();
      return Items.FirstOrDefault(x => x.IntegrationAlias == integration.Alias) as T;
    }


    /// <inheritdoc />
    public async Task<IIntegration> GetByAlias(string alias)
    {
      IIntegrationType integration = RegisteredTypes.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));

      if (integration == null)
      {
        return default;
      }

      await Preload();
      return Items.FirstOrDefault(x => x.IntegrationAlias == integration.Alias);
    }


    /// <inheritdoc />
    public async Task<T> GetById<T>(string id) where T : class, IIntegration
    {
      return await GetById(id) as T;
    }
  }


  public interface IIntegrationsCollection : ICollectionBase<IIntegration>
  {
    /// <summary>
    /// Get settings for a specific integration
    /// </summary>
    Task<T> Get<T>() where T : class, IIntegration;

    /// <summary>
    /// Get settings for a specific integration
    /// </summary>
    Task<IIntegration> GetByAlias(string alias);

    /// <summary>
    /// Get settings for a specific integration
    /// </summary>
    Task<T> GetById<T>(string id) where T : class, IIntegration;
  }
}
