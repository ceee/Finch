namespace zero.Identity;

public abstract class PermissionProvider : IPermissionProvider
{
  /// <inheritdoc />
  public virtual Task Configure(IPermissionContext context) => Task.CompletedTask;
}


public interface IPermissionProvider
{
  /// <summary>
  /// Manage permissions (add/remove/update permission groups and containing permissions)
  /// </summary>
  Task Configure(IPermissionContext context);

  /// <summary>
  /// In order for this permissions to work, the specified provider requires the following permission
  /// </summary>
  //IEnumerable<string> Requires() => Array.Empty<string>();
}