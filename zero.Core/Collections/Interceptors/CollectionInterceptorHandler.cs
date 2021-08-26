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
    protected ConcurrentDictionary<Type, ICollectionInterceptor> Interceptors { get; private set; } = new();

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
      where TParameters : CollectionInterceptor.Parameters<T>
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
    internal async Task HandleBefore<T, TParameters>(Expression<Func<ICollectionInterceptor, Task<InterceptorResult<T>>>> expression, InterceptorInstruction<T, TParameters> instruction)
      where T : ZeroEntity
      where TParameters : CollectionInterceptor.Parameters<T>
    {
      Func<ICollectionInterceptor, Task<InterceptorResult<T>>> func = default;

      foreach (InterceptorRegistration registration in ForType(typeof(T)))
      {
        if (!TryResolve(registration, out ICollectionInterceptor interceptor))
        {
          continue;
        }

        if (func == default)
        {
          func = expression.Compile();
        }

        Logger.LogDebug("Run interceptor {interceptor} before operation {operation}", registration.Name, instruction.Operation);

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
    internal async Task HandleAfter<T, TParameters>(Expression<Func<ICollectionInterceptor, Task>> expression, InterceptorInstruction<T, TParameters> instruction)
      where T : ZeroEntity
      where TParameters : CollectionInterceptor.Parameters<T>
    {
      Func<ICollectionInterceptor, Task> func = default;
      instruction ??= new();

      foreach (InterceptorRegistration registration in ForType(typeof(T)))
      {
        if (!TryResolve(registration, out ICollectionInterceptor interceptor))
        {
          continue;
        }

        InterceptorResult<T> beforeResult = instruction?.Results.FirstOrDefault(res => res.InterceptorHash == registration.Hash);

        if (func == default)
        {
          func = expression.Compile();
        }

        Logger.LogDebug("Run interceptor {interceptor} after operation {operation}", registration.Name, instruction.Operation);

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
    bool TryResolve(InterceptorRegistration registration, out ICollectionInterceptor interceptor)
    {
      Type type = registration.InterceptorType;

      if (Interceptors.TryGetValue(type, out interceptor))
      {
        return true;
      }

      interceptor = Services.GetService(type) as ICollectionInterceptor;

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
      where TParameters : CollectionInterceptor.Parameters<T>;
  }
}
