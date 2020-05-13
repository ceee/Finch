using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Options
{
  public abstract class ZeroBackofficeCollection<T>
  {
    protected IList<T> Items { get; set; } = new List<T>();
  }
}
