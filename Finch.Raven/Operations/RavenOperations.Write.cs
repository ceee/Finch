using FluentValidation.Results;
using Rv = Raven.Client;

namespace Finch.Raven;

public partial class RavenOperations : IRavenOperations
{
  /// <inheritdoc />
  public virtual Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null, Action<IAsyncDocumentSession> onAfterStore = null) where T : FinchIdEntity, new() => Save(model, validate, onAfterStore);

  /// <inheritdoc />
  public virtual Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null, Action<IAsyncDocumentSession> onAfterStore = null) where T : FinchIdEntity, new() => Save(model, validate, onAfterStore, true);

  /// <inheritdoc />
  protected virtual async Task<Result<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null, Action<IAsyncDocumentSession> onAfterStore = null, bool update = false) where T : FinchIdEntity, new()
  {
    if (model == null)
    {
      return Result<T>.Fail("@errors.onsave.empty");
    }

    T previousModel = null;

    // check if the Id for a model already exists
    if (!model.Id.IsNullOrEmpty())
    {
      using (IAsyncDocumentSession session = Store.Session(options: new Rv.Documents.Session.SessionOptions() { NoCaching = true }))
      {
        previousModel = await session.LoadAsync<T>(model.Id);
      }

      if (update && previousModel == null)
      {
        return Result<T>.Fail("@errors.onsave.noidmatch");
      }
      else if (!update && previousModel != null)
      {
        return Result<T>.Fail("@errors.oncreate.idmismatch");
      }
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
    if (!update)
    {
      model.Id = await GenerateId(model);
    }

    // run interceptor
    InterceptorInstruction<T> instruction = update ? Interceptors.ForUpdate(model, previousModel) : Interceptors.ForCreate(model);

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
  public async Task<Result<IOrderedEnumerable<T>>> Sort<T>(string[] sortedIds) where T : FinchIdEntity, ISupportsSorting, new()
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


  /// <inheritdoc />
  public virtual async Task<Result<IEnumerable<T>>> CreateAll<T>(IEnumerable<T> models) where T : FinchIdEntity, new()
  {
    using var bulkInsert = Store.Raven.BulkInsert();

    foreach (T model in models)
    {
      // prepare model
      PrepareForSave(model);

      // create ID before-hand so interceptors can use it
      model.Id = await GenerateId(model);

      // run interceptor
      InterceptorInstruction<T> instruction = Interceptors.ForCreate(model);

      if (InterceptorBlocker == null && !await instruction.Start(this))
      {
        await bulkInsert.AbortAsync();
        return instruction.Result.ConvertTo<IEnumerable<T>>(null);
      }

      // store our model
      await bulkInsert.StoreAsync(model);
      if (InterceptorBlocker == null)
      {
        await instruction.Complete();
      }
    }

    return Result<IEnumerable<T>>.Success(models);
  }
}