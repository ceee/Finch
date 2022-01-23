using FluentValidation;
using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Configuration;

public class IntegrationStore : IIntegrationStore
{
  protected IStoreOperations Operations { get; private set; }

  protected IIntegrationTypeService IntegrationTypes { get; private set; }


  public IntegrationStore(IStoreOperationsWithInactive operations, IIntegrationTypeService integrationTypes)
  {
    IntegrationTypes = integrationTypes;
    Operations = operations;
  }


  /// <inheritdoc />
  public virtual async Task<Integration> Empty(string alias)
  {
    return await Operations.Empty<Integration>(alias);
  }


  /// <inheritdoc />
  public virtual async Task<Integration> Load(string alias)
  {
    Paged<Integration> result = await Operations.Load<Integration>(1, 1, q => q.Where(x => x.Flavor == alias));
    return result.Items.FirstOrDefault();
  }


  /// <inheritdoc />
  public async Task<IList<Integration>> LoadByTag(string tag)
  {
    IEnumerable<IntegrationType> types = IntegrationTypes.GetByTag(tag);

    if (!types.Any())
    {
      return new List<Integration>();
    }

    string[] aliases = types.Select(x => x.Alias).ToArray();
    return await Operations.Session.Query<Integration>().Where(x => x.Flavor.In(aliases)).ToListAsync();
  }


  /// <inheritdoc />
  public async Task<bool> Any(string tag)
  {
    IEnumerable<IntegrationType> types = IntegrationTypes.GetByTag(tag);

    if (!types.Any())
    {
      return false;
    }

    string[] aliases = types.Select(x => x.Alias).ToArray();
    return await Operations.Session.Query<Integration>().AnyAsync(x => x.Flavor.In(aliases) && x.IsActive);
  }


  /// <inheritdoc />
  public virtual string GetChangeToken(Integration model) => Operations.GetChangeToken(model);


  /// <inheritdoc />
  public virtual async Task<Result<Integration>> Create(Integration model) => await Save(model, true);


  /// <inheritdoc />
  public virtual async Task<Result<Integration>> Update(Integration model) => await Save(model, false);


  /// <inheritdoc />
  public virtual async Task<Result<Integration>> Activate(string alias)
  {
    Integration model = await Load(alias);
    if (model != null)
    {
      model.IsActive = true;
    }
    return await Update(model);
  }


  /// <inheritdoc />
  public virtual async Task<Result<Integration>> Deactivate(string alias)
  {
    Integration model = await Load(alias);
    if (model != null)
    {
      model.IsActive = false;
    }
    return await Update(model);
  }



  /// <inheritdoc />
  public async Task<Result<Integration>> Delete(string alias)
  {
    Integration integration = await Load(alias);
    return await Operations.Delete(integration);
  }


  // <inheritdoc />
  protected virtual async Task<ValidationResult> Validate(Integration model, bool create = false)
  {
    ValidationResult result = new();

    if (model == null)
    {
      result.Errors.Add(new ValidationFailure("__nofield", "@integration.errors.notfound"));
    }
    else
    {
      IntegrationType type = IntegrationTypes.GetByAlias(model.Flavor);

      if (type == null)
      {
        result.Errors.Add(new ValidationFailure("__nofield", "@integration.errors.typenotfound"));
      }

      if (create && await Operations.Any<Integration>(q => q.Where(x => x.Flavor == model.Flavor)))
      {
        result.Errors.Add(new ValidationFailure("__nofield", "@integration.errors.multiplenotallowed"));
      }

      if (!create && await Operations.Any<Integration>(q => q.Where(x => x.Flavor == model.Flavor && x.Id != model.Id)))
      {
        result.Errors.Add(new ValidationFailure("__nofield", "@integration.errors.alreadycreated"));
      }

      if (type != null && type.Validator != null)
      {
        ValidationResult innerResult = await type.Validator.ValidateAsync(new ValidationContext<Integration>(model));
        result.Errors.AddRange(innerResult.Errors);
      }
    }

    return result;
  }


  /// <inheritdoc />
  protected virtual async Task<Result<Integration>> Save(Integration model, bool create = false)
  {
    return await Operations.Create(model, async x => await Validate(x, create));
  }
}


public interface IIntegrationStore
{
  /// <summary>
  /// Get new instance of an integration by integration alias
  /// </summary>
  Task<Integration> Empty(string alias);

  /// <summary>
  /// Get an integration by integration alias
  /// </summary>
  Task<Integration> Load(string alias);

  /// <summary>
  /// Get all integrations by a certain tag
  /// </summary>
  Task<IList<Integration>> LoadByTag(string tag);

  /// <summary>
  /// Check if there are any activated integrations for a certain tag
  /// </summary>
  Task<bool> Any(string tag);

  /// <summary>
  /// Get the change vector for a model (Proxy to IAsyncDocumentSession.GetChangeVectorFor<>)
  /// </summary>
  string GetChangeToken(Integration model);

  /// <summary>
  /// Creates a new integration
  /// </summary>
  Task<Result<Integration>> Create(Integration model);

  /// <summary>
  /// Updates an integration
  /// </summary>
  Task<Result<Integration>> Update(Integration model);

  /// <summary>
  /// Activates a configured integration
  /// </summary>
  Task<Result<Integration>> Activate(string alias);

  /// <summary>
  /// Disables a configured integration
  /// </summary>
  Task<Result<Integration>> Deactivate(string alias);

  /// <summary>
  /// Deletes configuration of an integration and disables it
  /// </summary>
  Task<Result<Integration>> Delete(string alias);
}