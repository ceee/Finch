using Microsoft.Extensions.DependencyInjection;
using System;

namespace zero.Core.Handlers
{
  public class HandlerHolder : IHandlerHolder
  {
    IServiceProvider Services { get; }

    public HandlerHolder(IServiceProvider services)
    {
      Services = services;
    }


    public T Get<T>() where T : IHandler
    {
      return Services.GetService<T>();
    }
  }


  public interface IHandlerHolder
  {
    T Get<T>() where T : IHandler;
  }
}
