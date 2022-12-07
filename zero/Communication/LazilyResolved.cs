using Microsoft.Extensions.DependencyInjection;

namespace zero.Communication;

public class LazilyResolved<T> : Lazy<T>
{
  public LazilyResolved(IServiceProvider serviceProvider): base(serviceProvider.GetRequiredService<T>)
  {

  }
}