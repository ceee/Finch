namespace zero.Identity;

public class PermissionContext : IPermissionContext
{
  /// <inheritdoc />
  public HashSet<PermissionGroup> Groups { get; protected set; }

  /// <inheritdoc />
  public IZeroOptions Options { get; protected set; }


  public PermissionContext(IZeroOptions options)
  {
    Groups = new();
    Options = options;
  }


  /// <inheritdoc />
  public PermissionGroup? GetGroup(string key)
  {
    return Groups.FirstOrDefault(x => x.Key == key);
  }


  /// <inheritdoc />
  public bool TryGetGroup(string key, out PermissionGroup group)
  {
    group = Groups.FirstOrDefault(x => x.Key == key);
    return Groups.Contains(group);
  }


  /// <inheritdoc />
  public void AddGroup(PermissionGroup group)
  {
    Groups.Add(group);
  }


  /// <inheritdoc />
  public void RemoveGroup(PermissionGroup group)
  {
    Groups.Remove(group);
  }
}


public interface IPermissionContext
{
  /// <summary>
  /// All registered permission groups
  /// </summary>
  HashSet<PermissionGroup> Groups { get; }

  /// <summary>
  /// Zero options
  /// </summary>
  IZeroOptions Options { get; }

  /// <summary>
  /// Try to get a permission group by key
  /// </summary>
  PermissionGroup? GetGroup(string key);

  /// <summary>
  /// Get permission group by key
  /// </summary>
  bool TryGetGroup(string key, out PermissionGroup group);

  /// <summary>
  /// Add a new permission group to the context
  /// </summary>
  void AddGroup(PermissionGroup group);

  /// <summary>
  /// Remove a permission group from the context
  /// </summary>
  void RemoveGroup(PermissionGroup group);
}