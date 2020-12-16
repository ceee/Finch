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
  public class IntegrationTypeService : IIntegrationTypeService
  {
    protected IZeroOptions Options { get; private set; }

    protected IZeroStore Store { get; private set; }


    public IntegrationTypeService(IZeroOptions options, IZeroStore store)
    {
      Options = options;
      Store = store;
    }


    /// <inheritdoc />
    public IIntegrationType GetByAlias(string alias)
    {
      return GetAll().FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public IIntegrationType GetBy<T>() where T : class, IIntegration
    {
      Type type = typeof(T);
      return GetAll().FirstOrDefault(x => x.SettingsType == type);
    }


    /// <inheritdoc />
    public IReadOnlyCollection<IIntegrationType> GetAll()
    {
      return Options.Integrations.GetAllItems();
    }


    /// <inheritdoc />
    public async Task<IList<IIntegrationType>> GetActivated()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      string[] activated = await session.Query<IIntegration>().Where(x => x.IsActive).Select(x => x.IntegrationAlias).ToArrayAsync();

      return GetAll().Where(x => x.IsAutoActivated || activated.Contains(x.Alias)).ToList();
    }


    /// <inheritdoc />
    public async Task<IList<IIntegrationType>> GetAvailable()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      string[] activated = await session.Query<IIntegration>().Where(x => x.IsActive).Select(x => x.IntegrationAlias).ToArrayAsync();

      return GetAll().Where(x => x.IsAutoActivated || !activated.Contains(x.Alias)).ToList();
    }
  }


  public interface IIntegrationTypeService
  {
    /// <summary>
    /// Get an integration by the specified alias
    /// </summary>
    IIntegrationType GetByAlias(string alias);

    /// <summary>
    /// Get an integration by the settings type
    /// </summary>
    IIntegrationType GetBy<T>() where T : class, IIntegration;

    /// <summary>
    /// Get all available app integrations
    /// </summary>
    IReadOnlyCollection<IIntegrationType> GetAll();

    /// <summary>
    /// Get already activated app integrations
    /// </summary>
    Task<IList<IIntegrationType>> GetActivated();

    /// <summary>
    /// Get available app integrations
    /// </summary>
    Task<IList<IIntegrationType>> GetAvailable();
  }
}
