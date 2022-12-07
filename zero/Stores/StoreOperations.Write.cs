using FluentValidation.Results;

namespace zero.Stores;

public partial class StoreOperations : IStoreOperations
{
  /// <inheritdoc />
  public virtual Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null, Action<IZeroDocumentSession> onAfterStore = null) where T : ZeroIdEntity, new() => Save(model, validate, onAfterStore);

  /// <inheritdoc />
  public virtual Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null, Action<IZeroDocumentSession> onAfterStore = null) where T : ZeroIdEntity, new() => Save(model, validate, onAfterStore);

  /// <inheritdoc />
  protected virtual async Task<Result<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null, Action<IZeroDocumentSession> onAfterStore = null) where T : ZeroIdEntity, new()
  {
    if (model == null)
    {
      return Result<T>.Fail("@errors.onsave.empty");
    }

    T previousModel = null;
    bool isUpdate = false;

    // check if the Id for a model already exists
    if (!model.Id.IsNullOrEmpty())
    {
      using (IZeroDocumentSession session = Context.Store.Session(resolution: ZeroSessionResolution.Create, options: new Raven.Client.Documents.Session.SessionOptions() { Database = Context.Store.ResolvedDatabase, NoCaching = true }))
      {
        previousModel = await session.LoadAsync<T>(model.Id);
      }

      if (previousModel == null)
      {
        return Result<T>.Fail("@errors.onsave.noidmatch");
      }

      isUpdate = true;
    }

    // validate flavor
    if (model is ISupportsFlavors flavorModel && !flavorModel.Flavor.IsNullOrEmpty())
    {
      if (!Flavors.Exists<T>(flavorModel.Flavor))
      {
        return Result<T>.Fail("@errors.onsave.flavornotfound");
      }   
    }

    // prepare model
    PrepareForSave(model);

    // run validator
    if (validate != null)
    {
      ValidationResult validation = await validate(model);

      if (!validation.IsValid)
      {
        return Result<T>.Fail(validation);
      }
    }

    // create ID before-hand so interceptors can use it
    if (!isUpdate)
    {
      model.Id = await GenerateId(model);
    }

    // run interceptor
    InterceptorInstruction<T> instruction = isUpdate ? Interceptors.ForUpdate(model, previousModel) : Interceptors.ForCreate(model);

    if (InterceptorBlocker == null && !await instruction.Start(this))
    {
      return instruction.Result;
    }

    // store our model
    await Session.StoreAsync(model);
    onAfterStore?.Invoke(Session);
    await Session.SaveChangesAsync();
    if (InterceptorBlocker == null)
    {
      await instruction.Complete();
    }
    await Session.SaveChangesAsync();

    return Result<T>.Success(model);
  }


  /// <inheritdoc />
  public async Task<Result<IOrderedEnumerable<T>>> Sort<T>(string[] sortedIds) where T : ZeroIdEntity, ISupportsSorting, new()
  {
    Dictionary<string, T> items = await Load<T>(sortedIds);
    uint index = 10;

    // contains multiple parents, therefore fail
    if (typeof(ISupportsTrees).IsAssignableFrom(typeof(T)) && items.Select(x => (x.Value as ISupportsTrees)?.ParentId).Distinct().Count() > 1)
    {
      return Result<IOrderedEnumerable<T>>.Fail("@errors.treeentity.sortingmultipleparents");
    }

    foreach (var item in items)
    {
      item.Value.Sort = index;
      index += 10;
      await Update(item.Value);
    }

    return Result<IOrderedEnumerable<T>>.Success(items.Select(x => x.Value).OrderByDescending(x => x.Sort));
  }
}