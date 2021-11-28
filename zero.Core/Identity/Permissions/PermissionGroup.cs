namespace zero.Identity;

public struct PermissionGroup
{
  /// <summary>
  /// Full key of the group
  /// </summary>
  public string Key { get; set; }

  /// <summary>
  /// Title of this permission group for output
  /// </summary>
  public string Label { get; set; }

  /// <summary>
  /// Permissions for this group
  /// </summary>
  public HashSet<Permission> Permissions { get; set; }


  public PermissionGroup(string key, string label)
  {
    Key = key;
    Label = label;
    Permissions = new();
  }


  /// <summary>
  /// Add a new permission to this group
  /// </summary>
  public void Add(Permission permission)
  {
    Permissions.Add(permission);
  }


  /// <summary>
  /// Remove a permission
  /// </summary>
  public void Remove(Permission permission)
  {
    Permissions.Remove(permission);
  }


  /// <summary>
  /// Get permission by key
  /// </summary>
  public Permission? Get(string key)
  {
    return Permissions.FirstOrDefault(x => x.Key == key);
  }


  /// <summary>
  /// Get permission by key
  /// </summary>
  public object this[string key]
  {
    get => Permissions.FirstOrDefault(x => x.Key == key);
  }
}