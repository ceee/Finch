using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using ServiceStack.OrmLite;
using zero.Models;
using zero.Extensions;

namespace zero.Sqlite;

public partial class DbOperations : IDbOperations
{
  /// <inheritdoc />
  public virtual Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new() => Save(model, validate);

  /// <inheritdoc />
  public virtual Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new() => Save(model, validate, true);

  /// <inheritdoc />
  protected virtual async Task<Result<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null, bool update = false) where T : ZeroIdEntity, new()
  {
    if (model == null)
    {
      return Result<T>.Fail("@errors.onsave.empty");
    }

    T previousModel = null;

    // check if the Id for a model already exists
    if (!model.Id.IsNullOrEmpty())
    {
      previousModel = await Db.SingleByIdAsync<T>(model.Id);

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

    // store our model
    await Db.SaveAsync(model);

    return Result<T>.Success(model);
  }
}