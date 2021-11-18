namespace zero.Core;

using System;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;

public class InterceptorInstruction<T> where T : ZeroIdEntity
{
  public Guid Guid { get; private set; } = Guid.NewGuid();

  public InterceptorType InterceptorType { get; private set; }

  public T Model { get; private set; }


  internal InterceptorInstruction(InterceptorType type, T model)
  {
    InterceptorType = type;
    Model = model;
  }


  public async Task<EntityResult<T>> Run()
  {
    //ICollectionInterceptor<T> interceptor = default;

    //interceptor.Created(new InterceptorParameters()
    //{
      
    //})

    await Task.Delay(0);
    return new EntityResult<T>();
  }


  public async Task Complete()
  {
    await Task.Delay(0);
  }
}