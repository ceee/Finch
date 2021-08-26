using System;
using zero.Core.Collections;
using zero.Core.Utils;

namespace zero.Core.Options
{
  public class InterceptorOptions : ZeroBackofficeCollection<InterceptorRegistration>, IZeroCollectionOptions
  {
    public void Add<T>(int gravity = 0, Func<Type, bool> canHandle = null) where T : ICollectionInterceptor
    {
      Items.Add(new InterceptorRegistration()
      {
        Hash = IdGenerator.Create(),
        Name = typeof(T).Name,
        Gravity = gravity,
        CanHandle = canHandle ?? (_ => true),
        InterceptorType = typeof(T)
      });
    }
  }


  public class InterceptorRegistration
  {
    public int Gravity { get; set; }

    public Type InterceptorType { get; set; }

    public string Hash { get; set; }

    public string Name { get; set; }

    public Func<Type, bool> CanHandle { get; set; }
  }
}
