using System;
using System.Collections.Generic;

namespace zero.Core.Options
{
  public class TypeOptions : IZeroCollectionOptions
  {
    Dictionary<Type, Type> Items { get; set; } = new Dictionary<Type, Type>();

    internal void Add<T, TTarget>() where TTarget : T
    {
      Items[typeof(T)] = typeof(TTarget);
    }
  }
}
