namespace zero.Stores;

public abstract partial class StoreOperations
{
  /// <inheritdoc />
  public virtual async Task<EntityResult<T>> Delete<T>(T model) where T : ZeroIdEntity, new()
  {
    if (model == null)
    {
      return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
    }

    InterceptorInstruction<T> instruction = Interceptors.ForDelete(model);

    if (!await instruction.Start())
    {
      return instruction.Result;
    }

    Session.Delete(model);
    await instruction.Complete();
    await Session.SaveChangesAsync();

    return EntityResult<T>.Success();
  }
}