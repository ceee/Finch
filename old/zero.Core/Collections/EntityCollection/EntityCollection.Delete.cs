namespace zero.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

public abstract partial class EntityCollection<T>
{
  /// <inheritdoc />
  public virtual async Task<EntityResult<T>> Delete(string id)
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

    InterceptorInstruction<T> instruction = Interceptors.CreateInstruction(this, InterceptorType.Delete, entity);

    if (!await instruction.Run())
    { 
      return instruction.Result;
    }

    Session.Delete(entity);

    await instruction.Complete();

    await Session.SaveChangesAsync();

    return EntityResult<T>.Success();
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<T>> Delete(T model) => await Delete(model?.Id);
  

  /// <inheritdoc />
  public virtual async Task<int> Delete(IEnumerable<T> models) => await Delete(models.Select(x => x.Id));


  /// <inheritdoc />
  public virtual async Task<int> Delete(IEnumerable<string> ids)
  {
    int successCount = 0;

    foreach (string id in ids)
    {
      EntityResult<T> result = await Delete(id);
      successCount += result.IsSuccess ? 1 : 0;
    }

    return successCount;
  }
}