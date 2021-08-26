using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public class InterceptorInstruction<T, TParameters>
    where T : ZeroEntity
    where TParameters : CollectionInterceptor<T>.Parameters
  {
    public string Operation { get; set; }

    public TParameters Parameters { get; set; }

    public List<InterceptorResult<T>> Results { get; set; } = new();

    public EntityResult<T> EntityResult { get; set; }

    public bool Return => EntityResult != null;

    internal Func<Expression<Func<ICollectionInterceptor<T>, Task<InterceptorResult<T>>>>, Task> BeforeOperationHandler = _ => Task.CompletedTask;

    internal Func<Expression<Func<ICollectionInterceptor<T>, Task>>, Task> AfterOperationHandler = _ => Task.CompletedTask;


    internal InterceptorInstruction() { }

    internal InterceptorInstruction(TParameters parameters)
    {
      Parameters = parameters;
    }


    public async Task HandleBefore(Expression<Func<ICollectionInterceptor<T>, Task<InterceptorResult<T>>>> expression)
    {
      await BeforeOperationHandler(expression);
    }

    public async Task HandleAfter(Expression<Func<ICollectionInterceptor<T>, Task>> expression)
    {
      await AfterOperationHandler(expression);
    }
  }
}
