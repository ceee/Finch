using FluentValidation.Results;

namespace zero.Stores;

public abstract partial class StoreOperations
{
  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new() => Save(model, validate);

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new() => Save(model, validate);

  /// <inheritdoc />
  protected virtual async Task<EntityResult<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new()
  {
    if (model == null)
    {
      return EntityResult<T>.Fail("@errors.onsave.empty");
    }

    bool isUpdate = !model.Id.IsNullOrEmpty() && await Session.Advanced.ExistsAsync(model.Id);

    // prepare model
    PrepareForSave(model);

    // run validator
    if (validate != null)
    {
      ValidationResult validation = await validate(model);
      if (!validation.IsValid)
      {
        return EntityResult<T>.Fail(validation);
      }
    }

    // create ID before-hand so interceptors can use it
    if (!isUpdate)
    {
      model.Id = await GenerateId(model);
    }

    // run interceptor
    InterceptorInstruction<T> instruction = isUpdate ? Interceptors.ForUpdate(model) : Interceptors.ForCreate(model);

    if (!await instruction.Start())
    {
      return instruction.Result;
    }

    // store our model
    await Session.StoreAsync(model);
    await instruction.Complete();
    await Session.SaveChangesAsync();

    return EntityResult<T>.Success(model);
  }
}