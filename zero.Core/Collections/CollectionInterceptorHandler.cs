using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Utils;

namespace zero.Core.Collections
{
  public class CollectionInterceptorHandler : ICollectionInterceptorHandler
  {
    protected ConcurrentBag<InterceptorRef> Interceptors { get; private set; } = new();

    protected ILogger<ICollectionInterceptorHandler> Logger { get; set; }


    public CollectionInterceptorHandler(IEnumerable<ICollectionInterceptor> items, ILogger<ICollectionInterceptorHandler> logger)
    {
      Logger = logger;
      Interceptors = new();

      foreach (ICollectionInterceptor item in items)
      {
        Interceptors.Add(new()
        {
          Interceptor = item,
          Hash = IdGenerator.Create(),
          Name = item.GetType().Name,
          Gravity = item.Gravity
        });
      }
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

      foreach (InterceptorRef interceptorRef in ForType(typeof(T)))
      {
        if (func == default)
        {
          func = expression.Compile();
        }

        Logger.LogDebug("Run interceptor {interceptor} before operation {operation}", interceptorRef.Name, instruction.Operation);

        InterceptorResult<T> result = await func(interceptorRef.Interceptor);

        if (result == default)
        {
          result = new();
        }

        result.InterceptorHash = interceptorRef.Hash;
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

      foreach (InterceptorRef interceptorRef in ForType(typeof(T)))
      {
        InterceptorResult<T> beforeResult = instruction?.Results.FirstOrDefault(res => res.InterceptorHash == interceptorRef.Hash);

        if (func == default)
        {
          func = expression.Compile();
        }

        Logger.LogDebug("Run interceptor {interceptor} after operation {operation}", interceptorRef.Name, instruction.Operation);

        await func(interceptorRef.Interceptor);
      }
    }


    /// <summary>
    /// Get all interceptors for a certain type
    /// </summary>
    IEnumerable<InterceptorRef> ForType(Type targetType)
    {
      return Interceptors.Where(map => map.Interceptor.CanHandle(targetType)).OrderByDescending(x => x.Gravity);
     // return Interceptors.Where(item => item.Types.Count == 0 || item.Types.Any(type => targetType.IsAssignableFrom(type)));
    }


    protected class InterceptorRef
    {
      public string Hash { get; set; }

      public ICollectionInterceptor Interceptor { get; set; }

      public string Name { get; set; }

      public int Gravity { get; set; }
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
