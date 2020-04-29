using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class ListCollections : List<ListCollection>
  {
    public void Add<T>(string alias, string name, string description, string icon) where T : ListItem, new()
    {
      Add(new ListCollection()
      {
        Alias = alias,
        Name = name,
        Description = description,
        Icon = icon,
        Type = typeof(T)
      });
    }
  }
}
