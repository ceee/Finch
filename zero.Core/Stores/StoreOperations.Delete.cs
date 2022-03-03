namespace zero.Stores;

public partial class StoreOperations : IStoreOperations
{
  /// <inheritdoc />
  public virtual async Task<Result<T>> Delete<T>(T model) where T : ZeroIdEntity, new()
  {
    if (model == null)
    {
      return Result<T>.Fail("@errors.ondelete.idnotfound");
    }

    InterceptorInstruction<T> instruction = Interceptors.ForDelete(model);

    if (InterceptorBlocker == null && !await instruction.Start(this))
    {
      return instruction.Result;
    }

    Session.Delete(model);
    await Session.SaveChangesAsync();
    if (InterceptorBlocker == null)
    {
      await instruction.Complete();
    }
    await Session.SaveChangesAsync();

    return Result<T>.Success();
  }
}