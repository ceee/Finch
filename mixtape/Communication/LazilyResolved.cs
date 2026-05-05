using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Communication;

public class LazilyResolved<T> : Lazy<T>
{
  public LazilyResolved(IServiceProvider serviceProvider): base(serviceProvider.GetRequiredService<T>)
  {

  }
}