using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Integrations;

namespace zero.Core.Collections
{
  public class IntegrationsCollection : EntityCollection<Integration>, IIntegrationsCollection
  {
    /// <inheritdoc />
    public IReadOnlyCollection<IntegrationType> RegisteredTypes { get; private set; }

    protected ILogger<IntegrationsCollection> Logger { get; private set; }


    public IntegrationsCollection(ICollectionContext<Integration> context, ILogger<IntegrationsCollection> logger) : base(context)
    {
      Options = new(true);
      RegisteredTypes = Context.Options.Integrations.GetAllItems();
      Logger = logger;
    }


    /// <inheritdoc />
    public Integration GetEmpty(string alias)
    {
      IntegrationType type = RegisteredTypes.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));

      if (type == null)
      {
        return null;
      }

      try
      {
        Integration model = Activator.CreateInstance(type.ContentType) as Integration;
        model.TypeAlias = type.Alias;
        return model;
      }
      catch
      {
        Logger.LogWarning("Could not create integration with type {alias}", alias);
      }

      return null;
    }


    /// <inheritdoc />
    public async Task<Integration> GetByAlias(string alias)
    {
      IntegrationType type = RegisteredTypes.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
      return await Load<Integration>(type);
    }


    /// <inheritdoc />
    public async Task<T> Get<T>(string alias = null) where T : Integration, new()
    {
      IntegrationType type = RegisteredTypes.FirstOrDefault(x => x.ContentType == typeof(T) && (alias.IsNullOrEmpty() || x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase)));
      return await Load<T>(type);
    }


    /// <inheritdoc />
    public async Task<IList<Integration>> GetByTag(string tag)
    {
      IEnumerable<IntegrationType> types = RegisteredTypes.Where(x => x.Tags.Contains(tag, StringComparer.InvariantCultureIgnoreCase));

      if (!types.Any())
      {
        return new List<Integration>();
      }

      string[] aliases = types.Select(x => x.Alias).ToArray();
      return await Session.Query<Integration>().Where(x => x.TypeAlias.In(aliases)).ToListAsync();
    }


    /// <inheritdoc />
    public async Task<bool> Any(string tag)
    {
      return (await GetByTag(tag)).Any(x => x.IsActive);
    }


    /// <inheritdoc />
    public override async Task<Paged<Integration>> Load(ListQuery<Integration> query)
    {
      List<Integration> result = new();
      List<Integration> models = await Session.Query<Integration>().ToListAsync();

      foreach (IntegrationType type in RegisteredTypes)
      {
        Integration model = models.FirstOrDefault(x => x.TypeAlias == type.Alias);

        if (model != null)
        {
          model.Name = type.Name;
          result.Add(model);
        }
      }

      return result.ToQueriedList(query);
    }


    /// <inheritdoc />
    public async Task<IList<IntegrationTypeWithStatus>> GetTypesWithStatus()
    {
      List<IntegrationTypeWithStatus> result = new();
      List<Integration> models = await Session.Query<Integration>().ToListAsync();

      foreach (IntegrationType type in RegisteredTypes)
      {
        Integration model = models.FirstOrDefault(x => x.TypeAlias == type.Alias);

        result.Add(new()
        {
          Type = type,
          Id = model?.Id,
          IsActive = model?.IsActive ?? false,
          IsConfigured = model != null
        });
      }

      return result.OrderByDescending(x => x.IsActive).ThenByDescending(x => x.IsConfigured).ThenByDescending(x => x.Type.Name).ToList();
    }


    /// <inheritdoc />
    public override async Task<EntityResult<Integration>> Save(Integration model)
    {
      if (model == null)
      {
        return EntityResult<Integration>.Fail("@integration.errors.notfound");
      }

      IntegrationType type = RegisteredTypes.FirstOrDefault(x => x.Alias.Equals(model.TypeAlias, StringComparison.InvariantCultureIgnoreCase));

      if (type == null)
      {
        return EntityResult<Integration>.Fail("@integration.errors.typenotfound");
      }

      string existingId = await Session.Query<Integration>().Where(x => x.TypeAlias == type.Alias).Select(x => x.Id).FirstOrDefaultAsync();

      if (!existingId.IsNullOrEmpty() && existingId != model.Id)
      {
        return EntityResult<Integration>.Fail("@integration.errors.multiplenotallowed");
      }

      model.Alias = type.Alias;
      model.Name = null;

      EntityResult<Integration> result = await base.Save(model);

      if (result.IsSuccess)
      {
        result.Model.Name = type.Name;
      }

      return result;
    }


    /// <inheritdoc />
    public async Task<EntityResult<Integration>> Activate(string alias)
    {
      Integration model = await GetByAlias(alias);
      if (model != null)
      { 
        model.IsActive = true;
      }
      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Integration>> Deactivate(string alias)
    {
      Integration model = await GetByAlias(alias);
      if (model != null)
      {
        model.IsActive = false;
      }
      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Integration>> DeleteByAlias(string alias)
    {
      return await base.Delete(await GetByAlias(alias));
    }


    /// <summary>
    /// Get integration data from database
    /// </summary>
    protected async Task<T> Load<T>(IntegrationType type) where T : Integration, new()
    {
      if (type == null)
      {
        return default;
      }

      T entity = await Session.Query<T>().FirstOrDefaultAsync(x => x.TypeAlias == type.Alias);

      if (entity != null)
      {
        entity.Name = type.Name;
        entity.TypeAlias = type.Alias;
      }

      return WhenActive(entity) as T;
    }
  }


  public interface IIntegrationsCollection
  {
    /// <summary>
    /// Get all registered integration types
    /// </summary>
    IReadOnlyCollection<IntegrationType> RegisteredTypes { get; }

    /// <summary>
    /// Get new integration model for the specified integration type alias
    /// </summary>
    Integration GetEmpty(string alias);

    /// <summary>
    /// Get integration by an alias
    /// </summary>
    Task<Integration> GetByAlias(string alias);

    /// <summary>
    /// Get an integration by type and optional alias
    /// </summary>
    Task<T> Get<T>(string alias = null) where T : Integration, new();

    /// <summary>
    /// Get all integrations by a certain tag
    /// </summary>
    Task<IList<Integration>> GetByTag(string tag);

    /// <summary>
    /// Check if any integrations of certain tag are activated
    /// </summary>
    Task<bool> Any(string tag);

    /// <summary>
    /// Get all integrations with the specified query
    /// </summary>
    Task<Paged<Integration>> Load(ListQuery<Integration> query);

    /// <summary>
    /// Get all integration types with their configuration status
    /// </summary>
    Task<IList<IntegrationTypeWithStatus>> GetTypesWithStatus();

    /// <summary>
    /// Saves an integration
    /// </summary>
    Task<EntityResult<Integration>> Save(Integration model);

    /// <summary>
    /// Activates a configured integration
    /// </summary>
    Task<EntityResult<Integration>> Activate(string alias);

    /// <summary>
    /// Disables a configured integration
    /// </summary>
    Task<EntityResult<Integration>> Deactivate(string alias);

    /// <summary>
    /// Deletes configuration of an integration and disables it
    /// </summary>
    Task<EntityResult<Integration>> DeleteByAlias(string alias);
  }
}
