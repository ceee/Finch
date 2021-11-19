namespace zero.Core;

using System;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;

public class InterceptorInstruction<T> where T : ZeroIdEntity, new()
{
  public Guid Guid { get; private set; } = Guid.NewGuid();

  public InterceptorType InterceptorType { get; private set; }

  public T Model { get; private set; }

  public IEntityCollection<T> Collection { get;private set; }

  public InterceptorParameters<T> Parameters { get; private set; }

  public EntityResult<T> Result { get; private set; }


  internal InterceptorInstruction(IZeroContext context, IEntityCollection<T> collection, InterceptorType type, T model)
  {
    Collection = collection;
    InterceptorType = type;
    Model = model;

    Parameters = new InterceptorParameters<T>()
    {
      Context = context,
      Store = context.Store,
      Collection = collection,
      Properties = new()    
    };
  }


  public async Task<bool> Run()
  {
    ICollectionInterceptor<T> interceptor = default;

    //await HandleBefore(interceptor);
    //await interceptor.Created(Parameters, Model);
    //interceptor.Created(new InterceptorParameters()
    //{
      
    //})

    await Task.Delay(0);
    return true;
  }


  public async Task Complete()
  {
    await Task.Delay(0);
  }


  protected Task HandleBefore(ICollectionInterceptor<T> interceptor) => InterceptorType switch
  {
    InterceptorType.Save => interceptor.Saving(Parameters, Model),
    InterceptorType.Create => interceptor.Creating(Parameters, Model),
    InterceptorType.Update => interceptor.Updating(Parameters, Model),
    InterceptorType.Delete => interceptor.Deleting(Parameters, Model),
    _ => throw new NotImplementedException()
  };


  protected Task HandleAfter(ICollectionInterceptor<T> interceptor) => InterceptorType switch
  {
    InterceptorType.Save => interceptor.Saved(Parameters, Model),
    InterceptorType.Create => interceptor.Created(Parameters, Model),
    InterceptorType.Update => interceptor.Updated(Parameters, Model),
    InterceptorType.Delete => interceptor.Deleted(Parameters, Model),
    _ => throw new NotImplementedException()
  };
}