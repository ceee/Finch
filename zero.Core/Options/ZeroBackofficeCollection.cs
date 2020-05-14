using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace zero.Core.Options
{
  public abstract class ZeroBackofficeCollection<T>
  {
    protected List<T> Items { get; set; } = new List<T>();


    public IReadOnlyCollection<T> GetAllItems()
    {
      return Items.AsReadOnly();
    }
  }
}
