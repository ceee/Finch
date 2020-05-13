using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Options
{
  public class PermissionOptions : ZeroBackofficeCollection<PermissionCollection>, IZeroCollectionOptions
  {
    public PermissionOptions()
    {
      
    }


    public void AddCollection(PermissionCollection collection, int index = -1)
    {
      if (index > -1 && index < Items.Count)
      {
        Items.Insert(index, collection);
      }
      else
      {
        Items.Add(collection);
      }
    }


    public void Add(string collectionAlias, Permission permission, int index = -1)
    {
      PermissionCollection collection = Items.FirstOrDefault(x => x.Alias.Equals(collectionAlias, StringComparison.InvariantCultureIgnoreCase));

      if (collection == null)
      {
        // TODO handle error
        return;
      }

      IList<Permission> items = collection.Items;

      if (index > -1 && index < items.Count)
      {
        items.Insert(index, permission);
      }
      else
      {
        items.Add(permission);
      }
    }
  }
}
