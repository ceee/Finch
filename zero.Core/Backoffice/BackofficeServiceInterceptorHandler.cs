using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Backoffice
{
  public class BackofficeServiceInterceptorHandler : IBackofficeServiceInterceptorHandler
  {
    /// <inheritdoc />
    public IEnumerable<IBackofficeServiceInterceptor> Interceptors { get; private set; }


    public BackofficeServiceInterceptorHandler(IEnumerable<IBackofficeServiceInterceptor> items)
    {
      Interceptors = items;
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Handle<T>(Expression<Func<IBackofficeServiceInterceptor, Task<EntityResult<T>>>> intercept)
    {
      Func<IBackofficeServiceInterceptor, Task<EntityResult<T>>> func = default;

      foreach (IBackofficeServiceInterceptor interceptor in ForType(typeof(T)))
      {
        if (func == default)
        {
          func = intercept.Compile();
        }

        EntityResult<T> result = await func(interceptor);

        if (result != default)
        {
          return result;
        }
      }

      return default;
    }


    /// <inheritdoc />
    public async Task Handle<T>(Expression<Func<IBackofficeServiceInterceptor, Task>> intercept)
    {
      Func<IBackofficeServiceInterceptor, Task> func = default;

      foreach (IBackofficeServiceInterceptor interceptor in ForType(typeof(T)))
      {
        if (func == default)
        {
          func = intercept.Compile();
        }

        await func(interceptor);
      }
    }


    /// <summary>
    /// Get all interceptors for a certain type
    /// </summary>
    IEnumerable<IBackofficeServiceInterceptor> ForType(Type targetType)
    {
      return Interceptors.Where(item => item.Types.Count == 0 || item.Types.Any(type => targetType.IsAssignableFrom(type)));
    }
  }


  public interface IBackofficeServiceInterceptorHandler
  {
    /// <summary>
    /// All registered interceptors
    /// </summary>
    IEnumerable<IBackofficeServiceInterceptor> Interceptors { get; }

    /// <summary>
    /// Calls all matching interceptors with the specified expression
    /// </summary>
    Task<EntityResult<T>> Handle<T>(Expression<Func<IBackofficeServiceInterceptor, Task<EntityResult<T>>>> intercept);

    /// <summary>
    /// Calls all matching interceptors with the specified expression
    /// </summary>
    Task Handle<T>(Expression<Func<IBackofficeServiceInterceptor, Task>> intercept);
  }
}
