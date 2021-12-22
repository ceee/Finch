using FluentValidation.Results;

namespace zero.Configuration;

public class IntegrationStore : IIntegrationStore
{
  protected IStoreOperations Operations { get; private set; }

  protected IIntegrationTypeService IntegrationTypes { get; private set; }


  public IntegrationStore(IStoreContext context, IIntegrationTypeService integrationTypes)
  {
    IntegrationTypes = integrationTypes;
    Operations = new StoreOperations(context.Context, context.Interceptors, context.Options, new StoreConfig() { IncludeInactive = true });
  }


  /// <inheritdoc />
  public virtual Task<Integration> Empty(string alias)
  {
    IntegrationType type = IntegrationTypes.GetByAlias(alias);
    Integration integration = type?.Construct(type);

    if (integration != null)
    {
      integration.TypeAlias = alias;
    }

    return Task.FromResult(integration);
  }


  /// <inheritdoc />
  public virtual async Task<Integration> Load(string alias)
  {
    Paged<Integration> result = await Operations.Load<Integration>(1, 1, q => q.Where(x => x.TypeAlias == alias));
    return result.Items.FirstOrDefault();
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
  protected virtual async Task<ValidationResult> Validate(Integration model)
  {
    await Task.Delay(0);
    return new ValidationResult();
    //ZeroValidator<T> validator = new();
    //ValidationRules(validator);
    //return await validator.ValidateAsync(model);
    //return base.Validate(model);
  }


  /// <inheritdoc />
  protected virtual async Task<Result<Integration>> Save(Integration model, bool create = false)
  {
    if (model == null)
    {
      return Result<Integration>.Fail("@integration.errors.notfound");
    }

    IntegrationType type = IntegrationTypes.GetByAlias(model.TypeAlias);

    if (type == null)
    {
      return Result<Integration>.Fail("@integration.errors.typenotfound");
    }

    if (create && await Operations.Any<Integration>(q => q.Where(x => x.TypeAlias == model.TypeAlias)))
    {
      return Result<Integration>.Fail("@integration.errors.multiplenotallowed");
    }

    if (!create && await Operations.Any<Integration>(q => q.Where(x => x.TypeAlias == model.TypeAlias && x.Id != model.Id)))
    {
      return Result<Integration>.Fail("@integration.errors.alreadycreated");
    }

    return await Operations.Create(model, async x => await Validate(x));
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