namespace zero.Core;

using System;
using System.Collections.Concurrent;
using System.Linq;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Options;

public class InterceptorRunner<T> : IInterceptorRunner<T> where T : ZeroIdEntity, new()
{
  protected IZeroContext Context { get; set; }

  protected ConcurrentDictionary<Type, object> Interceptors { get; private set; } = new();


  public InterceptorRunner(IZeroContext context)
  {
    Context = context;
  }


  public InterceptorInstruction<T> CreateInstruction(EntityCollection<T> collection, InterceptorType type, T model)
  {
    return new InterceptorInstruction<T>(Context, collection, type, model);
  }


  /// <summary>
  /// Get all interceptors for a certain type
  /// </summary>
  IOrderedEnumerable<InterceptorRegistration> ForType(Type targetType)
  {
    return Context.Options.Interceptors.GetAllItems().Where(x => x.CanHandle(targetType)).OrderByDescending(x => x.Gravity);
    // return Interceptors.Where(item => item.Types.Count == 0 || item.Types.Any(type => targetType.IsAssignableFrom(type)));
  }


  /// <summary>
  /// Resolves an interceptor from the service provider
  /// </summary>
  bool TryResolve(InterceptorRegistration registration, out ICollectionInterceptor<T> interceptor)
  {
    Type type = registration.InterceptorType;

    if (Interceptors.TryGetValue(type, out object interceptorObj))
    {
      interceptor = interceptorObj as ICollectionInterceptor<T>;
      return interceptor != null;
    }

    object service = Context.Services.GetService(type);
    interceptor = service as ICollectionInterceptor<T>;

    if (interceptor == null && service != null && service is ICollectionInterceptor)
    {
      interceptor = new CollectionInterceptorShim<T>(service as ICollectionInterceptor);
    }

    Interceptors.TryAdd(type, interceptor);

    return interceptor != null;
  }
}


public interface IInterceptorRunner<T> where T : ZeroIdEntity, new()
{
  InterceptorInstruction<T> CreateInstruction(EntityCollection<T> collection, InterceptorType type, T model);
}