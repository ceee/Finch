using FluentValidation.Results;

namespace zero.Stores;

public partial class StoreOperations : IStoreOperations
{
  /// <inheritdoc />
  public virtual Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new() => Save(model, validate);

  /// <inheritdoc />
  public virtual Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new() => Save(model, validate);

  /// <inheritdoc />
  protected virtual async Task<Result<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new()
  {
    if (model == null)
    {
      return Result<T>.Fail("@errors.onsave.empty");
    }

    bool isUpdate = false;

    // check if the Id for a model already exists
    if (!model.Id.IsNullOrEmpty())
    {
      bool exists = await Session.Advanced.ExistsAsync(model.Id);

      if (!exists)
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
    InterceptorInstruction<T> instruction = isUpdate ? Interceptors.ForUpdate(model) : Interceptors.ForCreate(model);

    if (!await instruction.Start(this))
    {
      return instruction.Result;
    }

    // store our model
    await Session.StoreAsync(model);
    await instruction.Complete();
    await Session.SaveChangesAsync();

    return Result<T>.Success(model);
  }
}