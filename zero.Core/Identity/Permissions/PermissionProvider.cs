namespace zero.Identity;

public abstract class PermissionProvider : IPermissionProvider
{
  /// <inheritdoc />
  public string Name { get; private set; }


  public PermissionProvider(string name)
  {
    Name = name;
  }

  /// <inheritdoc />
  public virtual IEnumerable<Permission> GetPermissions() => new List<Permission>();


  /// <inheritdoc />
  public virtual Task<IEnumerable<Permission>> GetPermissionsAsync() => Task.FromResult(GetPermissions());
}


public interface IPermissionProvider
{
  /// <summary>
  /// Name (can be a localization) which is displayed in the permission group
  /// </summary>
  string Name { get; }

  /// <summary>
  /// Get all permissions for this provider.
  /// These permissions can be granted to roles or individual users.
  /// </summary>
  IEnumerable<Permission> GetPermissions();

  /// <summary>
  /// Get all permissions for this provider.
  /// These permissions can be granted to roles or individual users.
  /// </summary>
  Task<IEnumerable<Permission>> GetPermissionsAsync();
}