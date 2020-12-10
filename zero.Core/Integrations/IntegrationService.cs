using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Integrations
{
  public class IntegrationService : BackofficeApi, IIntegrationService
  {
    protected IZeroOptions Options { get; private set; }


    public IntegrationService(IZeroOptions options, IBackofficeStore bstore) : base(bstore)
    {
      Options = options;
    }


    /// <inheritdoc />
    public IIntegration GetByAlias(string alias)
    {
      return GetAll().FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public IIntegration GetBy<T>() where T : class, IIntegrationSettings
    {
      Type type = typeof(T);
      return GetAll().FirstOrDefault(x => x.SettingsType == type);
    }


    /// <inheritdoc />
    public IReadOnlyCollection<IIntegration> GetAll()
    {
      return Options.Integrations.GetAllItems();
    }


    /// <inheritdoc />
    public async Task<IList<IIntegration>> GetActivated()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      string[] activated = await session.Query<IIntegrationSettings>().Where(x => x.IsActive).Select(x => x.IntegrationAlias).ToArrayAsync();

      return GetAll().Where(x => x.IsAutoActivated || activated.Contains(x.Alias)).ToList();
    }


    /// <inheritdoc />
    public async Task<IList<IIntegration>> GetAvailable()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      string[] activated = await session.Query<IIntegrationSettings>().Where(x => x.IsActive).Select(x => x.IntegrationAlias).ToArrayAsync();

      return GetAll().Where(x => x.IsAutoActivated || !activated.Contains(x.Alias)).ToList();
    }


    /// <inheritdoc />
    public async Task<IIntegrationSettings> GetSettingsByAlias(string alias)
    {
      IIntegration integration = GetByAlias(alias);

      if (integration == null)
      {
        return default;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      return await session.Query<IIntegrationSettings>()
        .Where(x => x.IntegrationAlias == integration.Alias)
        .FirstOrDefaultAsync();
    }


    /// <inheritdoc />
    public async Task<IIntegrationSettings> GetSettingsById(string id)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session.LoadAsync<IIntegrationSettings>(id);
    }


    /// <inheritdoc />
    public async Task<T> GetSettingsById<T>(string id) where T : class, IIntegrationSettings
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session.LoadAsync<T>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IIntegrationSettings>> Save(IIntegrationSettings model)
    {
      return await SaveModel(model, null);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IIntegrationSettings>> Delete(string id)
    {
      return await DeleteById<IIntegrationSettings>(id);
    }
  }


  public interface IIntegrationService
  {
    /// <summary>
    /// Get an integration by the specified alias
    /// </summary>
    IIntegration GetByAlias(string alias);

    /// <summary>
    /// Get an integration by the settings type
    /// </summary>
    IIntegration GetBy<T>() where T : class, IIntegrationSettings;

    /// <summary>
    /// Get all available app integrations
    /// </summary>
    IReadOnlyCollection<IIntegration> GetAll();

    /// <summary>
    /// Get already activated app integrations
    /// </summary>
    Task<IList<IIntegration>> GetActivated();

    /// <summary>
    /// Get available app integrations
    /// </summary>
    Task<IList<IIntegration>> GetAvailable();

    /// <summary>
    /// Get settings for a specific integration
    /// </summary>
    Task<IIntegrationSettings> GetSettingsByAlias(string alias);

    /// <summary>
    /// Get settings for a specific integration
    /// </summary>
    Task<IIntegrationSettings> GetSettingsById(string id);

    /// <summary>
    /// Get settings for a specific integration
    /// </summary>
    Task<T> GetSettingsById<T>(string id) where T : class, IIntegrationSettings;

    /// <summary>
    /// Saves settings for an integration
    /// </summary>
    Task<EntityResult<IIntegrationSettings>> Save(IIntegrationSettings model);

    /// <summary>
    /// Deletes an integration (therefore its settings)
    /// </summary>
    Task<EntityResult<IIntegrationSettings>> Delete(string id);
  }
}
