using System.Collections.Generic;

namespace zero.Identity;

public class PermissionGroupCollection : List<PermissionCollection>
{
  public void Add<T>(int index = -1) where T : PermissionCollection, new()
  {
    if (index > -1 && index < Count)
    {
      Insert(index, new T());
    }
    else
    {
      Add(new T());
    }
  }
}