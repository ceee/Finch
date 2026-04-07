using Microsoft.Extensions.DependencyInjection;

namespace Finch.Communication;

public class LazilyResolved<T> : Lazy<T>
{
  public LazilyResolved(IServiceProvider serviceProvider): base(serviceProvider.GetRequiredService<T>)
  {

  }
}