using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Collections
{
  public class CollectionInterceptorHandler : ICollectionInterceptorHandler
  {
    protected ConcurrentDictionary<Type, object> Interceptors { get; private set; } = new();

    protected IZeroOptions Options { get; set; }

    protected IServiceProvider Services { get; set; }

    protected ILogger<ICollectionInterceptorHandler> Logger { get; set; }


    public CollectionInterceptorHandler(IZeroOptions options, IServiceProvider serviceProvider, ILogger<ICollectionInterceptorHandler> logger)
    {
      Logger = logger;
      Options = options;
      Services = serviceProvider;
    }


    /// <inheritdoc />
    public InterceptorInstruction<T, TParameters> Create<T, TParameters>(string operationName, TParameters parameters)
      where T : ZeroEntity
      where TParameters : CollectionInterceptor<T>.Parameters
    {
      InterceptorInstruction<T, TParameters> instruction = new(parameters);
      instruction.Operation = operationName;
      instruction.BeforeOperationHandler = async expression => await HandleBefore(expression, instruction);
      instruction.AfterOperationHandler = async expression => await HandleAfter(expression, instruction);

      return instruction;
    }



    /// <summary>
    /// Calls all matching interceptors with the specified expression
    /// </summary>
    internal async Task HandleBefore<T, TParameters>(Expression<Func<ICollectionInterceptor<T>, Task<InterceptorResult<T>>>> expression, InterceptorInstruction<T, TParameters> instruction)
      where T : ZeroEntity
      where TParameters : CollectionInterceptor<T>.Parameters
    {
      string typeName = (typeof(T)).Name;
      Func<ICollectionInterceptor<T>, Task<InterceptorResult<T>>> func = default;

      foreach (InterceptorRegistration registration in ForType(typeof(T)))
      {
        if (!TryResolve(registration, out ICollectionInterceptor<T> interceptor))
        {
          continue;
        }

        if (func == default)
        {
          func = expression.Compile();
        }

        // we do not log save operations as they are always called for update/create which are already logged beforehand
        if (instruction.Operation != "save")
        {
          Logger.LogDebug("Run interceptor {interceptor} for {type}:{operation}", registration.Name, typeName, instruction.Operation);
        }

        InterceptorResult<T> result = await func(interceptor);

        if (result == default)
        {
          result = new();
        }

        result.InterceptorHash = registration.Hash;
        instruction.Results.Add(result);

        if (result.Result != null)
        {
          instruction.EntityResult = result.Result;
          break;
        }
        if (result.Prevent)
        {
          break;
        }
      }
    }



    /// <summary>
    /// Calls all matching interceptors with the specified expression
    /// </summary>
    internal async Task HandleAfter<T, TParameters>(Expression<Func<ICollectionInterceptor<T>, Task>> expression, InterceptorInstruction<T, TParameters> instruction)
      where T : ZeroEntity
      where TParameters : CollectionInterceptor<T>.Parameters
    {
      Func<ICollectionInterceptor<T>, Task> func = default;
      instruction ??= new();

      foreach (InterceptorRegistration registration in ForType(typeof(T)))
      {
        if (!TryResolve(registration, out ICollectionInterceptor<T> interceptor))
        {
          continue;
        }

        InterceptorResult<T> beforeResult = instruction?.Results.FirstOrDefault(res => res.InterceptorHash == registration.Hash);

        if (func == default)
        {
          func = expression.Compile();
        }

        await func(interceptor);
      }
    }


    /// <summary>
    /// Get all interceptors for a certain type
    /// </summary>
    IOrderedEnumerable<InterceptorRegistration> ForType(Type targetType)
    {
      return Options.Interceptors.GetAllItems().Where(x => x.CanHandle(targetType)).OrderByDescending(x => x.Gravity);
     // return Interceptors.Where(item => item.Types.Count == 0 || item.Types.Any(type => targetType.IsAssignableFrom(type)));
    }


    /// <summary>
    /// Resolves an interceptor from the service provider
    /// </summary>
    bool TryResolve<T>(InterceptorRegistration registration, out ICollectionInterceptor<T> interceptor) where T : ZeroEntity
    {
      Type type = registration.InterceptorType;

      if (Interceptors.TryGetValue(type, out object interceptorObj))
      {
        interceptor = interceptorObj as ICollectionInterceptor<T>;
        return interceptor != null;
      }

      object service = Services.GetService(type);
      interceptor = service as ICollectionInterceptor<T>;

      if (interceptor == null && service != null && service is ICollectionInterceptor)
      {
        interceptor = new CollectionInterceptorShim<T>(service as ICollectionInterceptor);
      }

      if (interceptor == null)
      {
        Logger.LogWarning("Could not resolve interceptor {interceptor}", registration.Name);
      }

      Interceptors.TryAdd(type, interceptor);

      return interceptor != null;
    }
  }


  public interface ICollectionInterceptorHandler
  {
    /// <summary>
    /// Creates a new interceptor instruction
    /// </summary>
    InterceptorInstruction<T, TParameters> Create<T, TParameters>(string operationName, TParameters parameters)
      where T : ZeroEntity
      where TParameters : CollectionInterceptor<T>.Parameters;
  }
}
