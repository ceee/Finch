namespace zero.Core;

using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;

public abstract partial class EntityCollection<T> : IEntityCollection<T> where T : ZeroIdEntity
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

    var instruction = Interceptors.CreateInstruction(InterceptorType.Delete, entity);

    EntityResult<T> result = await instruction.Run();

    if (result != null)
    {
      return result;
    }

    //var instruction = CreateInstruction<CollectionInterceptor<T>.DeleteParameters>("delete", args =>
    //{
    //  args.Model = entity;
    //  args.Id = entity.Id;
    //});
    //await instruction.HandleBefore(x => x.Deleting(instruction.Parameters));

    //if (instruction.Return)
    //{
    //  return instruction.EntityResult;
    //}

    Session.Delete(entity);

    await instruction.Complete();
    //await instruction.HandleAfter(x => x.Deleted(instruction.Parameters));

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