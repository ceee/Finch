namespace zero.Stores;

public abstract partial class StoreOperations
{
  /// <inheritdoc />
  public virtual async Task<EntityResult<T>> Delete<T>(string id) where T : ZeroIdEntity, new()
  {
    if (id.IsNullOrEmpty())
    {
      return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
    }

    T entity = await Session.LoadAsync<T>(id);

    if (entity == null)
    {
      return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
    }

    InterceptorInstruction<T> instruction = Interceptors.ForDelete(entity);

    if (!await instruction.Start())
    { 
      return instruction.Result;
    }

    Session.Delete(entity);
    await instruction.Complete();
    await Session.SaveChangesAsync();

    return EntityResult<T>.Success();
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<T>> Delete<T>(T model) where T : ZeroIdEntity, new() => await Delete<T>(model?.Id);
  

  /// <inheritdoc />
  public virtual async Task<int> Delete<T>(IEnumerable<T> models) where T : ZeroIdEntity, new() => await Delete<T>(models.Select(x => x.Id));


  /// <inheritdoc />
  public virtual async Task<int> Delete<T>(IEnumerable<string> ids) where T : ZeroIdEntity, new()
  {
    int successCount = 0;

    foreach (string id in ids)
    {
      EntityResult<T> result = await Delete<T>(id);
      successCount += result.IsSuccess ? 1 : 0;
    }

    return successCount;
  }
}