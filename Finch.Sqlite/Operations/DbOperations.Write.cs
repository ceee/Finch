using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using ServiceStack.OrmLite;
using Finch.Models;
using Finch.Extensions;

namespace Finch.Sqlite;

public partial class DbOperations : IDbOperations
{
  /// <inheritdoc />
  public virtual Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : FinchIdEntity, new() => Save(model, validate);

  /// <inheritdoc />
  public virtual Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : FinchIdEntity, new() => Save(model, validate, true);

  /// <inheritdoc />
  public virtual async Task<Result<T>> CreateOrUpdate<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : FinchIdEntity, new()
  {
    bool update = !model.Id.IsNullOrEmpty() && await Any<T>(x => x.Id == model.Id);
    return await Save(model, validate, update);
  }

  /// <inheritdoc />
  protected virtual async Task<Result<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null, bool update = false) where T : FinchIdEntity, new()
  {
    if (model == null)
    {
      Logger.LogWarning("Could not create/update entity (model is null) for type {type}", typeof(T));
      return Result<T>.Fail("@errors.onsave.empty");
    }

    // check if the Id for a model already exists
    if (!model.Id.IsNullOrEmpty())
    {
      T previousModel = await Db.SingleByIdAsync<T>(model.Id);

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
        Logger.LogWarning("Flavor {flavor} not found for type {type}", flavorModel.Flavor, typeof(T));
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
        Logger.LogInformation("Validation failed for {id} ({errors})", model.Id, validation.Errors);
        return Result<T>.Fail(validation);
      }
    }

    // create ID before-hand so interceptors can use it
    if (!update && !model.Id.HasValue())
    {
      model.Id = await GenerateId(model);
    }

    // store our model
    await Db.SaveAsync(model);

    string action = update ? "Updated" : "Created";
    if (model is FinchEntity finchEntity)
    {
      Logger.LogInformation(action + " {id} with name {name}", model.Id, finchEntity.Name);
    }
    else
    {
      Logger.LogInformation(action + " {id}", model.Id);
    }

    await EntityModifiedHandler.Saved(model, update);

    return Result<T>.Success(model);
  }


  /// <inheritdoc />
  public virtual async Task Sort<T>(IEnumerable<string> ids) where T : FinchEntity, new()
  {
    List<T> items = await LoadAll<T>();

    uint sort = 0;
    foreach (string id in ids)
    {
      T item = items.FirstOrDefault(x => x.Id == id);
      if (item != null)
      {
        sort += 10;
        item.Sort = sort;
        item.LastModifiedDate = DateTimeOffset.Now;
      }
    }
    await Db.UpdateAllAsync(items);
    foreach (T item in items)
    {
      await EntityModifiedHandler.Updated(item);
    }
  }
}