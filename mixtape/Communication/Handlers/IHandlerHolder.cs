using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Communication;

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


  public IEnumerable<T> GetAll<T>() where T : IHandler
  {
    return Services.GetService<IEnumerable<T>>();
  }
}

public interface IHandlerHolder
{
  T Get<T>() where T : IHandler;

  IEnumerable<T> GetAll<T>() where T : IHandler;
}