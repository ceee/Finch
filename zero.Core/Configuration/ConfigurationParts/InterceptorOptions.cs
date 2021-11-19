using System;
using zero.Core.Collections;

namespace zero;

public class InterceptorOptions : OptionsEnumerable<InterceptorRegistration>, IOptionsEnumerable
{
  public void Add<T>(int gravity = 0, Func<Type, bool> canHandle = null) where T : ICollectionInterceptor
  {
    Type type = typeof(T);

    if (canHandle == null)
    {
      canHandle = _ => true;
    }

    Items.Add(new InterceptorRegistration()
    {
      Hash = IdGenerator.Create(),
      Name = type.Name,
      Gravity = gravity,
      CanHandle = canHandle,
      InterceptorType = type
    });
  }


  public void Add<T, TBoxed>(int gravity = 0, Func<Type, bool> canHandle = null) 
    where T : ICollectionInterceptor<TBoxed>
    where TBoxed : ZeroEntity
  {
    Type type = typeof(T);
    Type boxedType = typeof(TBoxed);
    Func<Type, bool> finalCanHandle = requestedType =>
    {
      return boxedType.IsAssignableFrom(requestedType) && (canHandle == null || canHandle.Invoke(requestedType));
    };

    Items.Add(new InterceptorRegistration()
    {
      Hash = IdGenerator.Create(),
      Name = type.Name,
      Gravity = gravity,
      CanHandle = finalCanHandle,
      InterceptorType = type,
      IsInterceptorBoxed = true
    });
  }
}


public class InterceptorRegistration
{
  public int Gravity { get; set; }

  public Type InterceptorType { get; set; }

  public string Hash { get; set; }

  public string Name { get; set; }

  public Func<Type, bool> CanHandle { get; set; }

  public bool IsInterceptorBoxed { get; set; }
}