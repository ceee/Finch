namespace zero.Configuration;

public class PermissionOptions : OptionsEnumerable<PermissionCollection>, IOptionsEnumerable
{
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


  public void AddCollection<T>(int index = -1) where T : PermissionCollection, new()
  {
    if (index > -1 && index < Items.Count)
    {
      Items.Insert(index, new T());
    }
    else
    {
      Items.Add(new T());
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