using System.Security.Claims;

namespace zero.Identity;

public struct Permission
{
  /// <summary>
  /// Full key (stored in left part claim value) of the permission
  /// </summary>
  public string Key { get; set; }

  /// <summary>
  /// Title of this permission for output
  /// </summary>
  public string Label { get; set; }

  /// <summary>
  /// When a permission is disabled it can not be updated anymore
  /// </summary>
  public bool IsDisabled { get; set; }

  /// <summary>
  /// Child permissions
  /// </summary>
  public HashSet<Permission> Children { get; set; }


  public Permission(string key, string label)
  {
    Key = key;
    Label = label;
    IsDisabled = false;
    Children = new();
  }

  ///// <summary>
  ///// Whether a resource (with authorization based on the key) can be read by any of the given permisssions
  ///// </summary>
  //public static bool CanReadKey(IEnumerable<Permission> permissions, string key, bool isNormalized = false)
  //{
  //  Permission permission = permissions.FirstOrDefault(p => isNormalized ? p.NormalizedKey == key : p.Key == key);
  //  return permission?.CanRead ?? false;
  //}

  ///// <summary>
  ///// Whether a resource (with authorization based on the key) can be written to by any of the given permisssions
  ///// </summary>
  //public static bool CanWriteKey(IEnumerable<Permission> permissions, string key, bool isNormalized = false)
  //{
  //  Permission permission = permissions.FirstOrDefault(p => isNormalized ? p.NormalizedKey == key : p.Key == key);
  //  return permission?.CanWrite ?? false;
  //}

  ///// <summary>
  ///// Create a permission from a claim
  ///// </summary>
  //public static Permission FromClaim(Claim claim, string prefixToRemove = null)
  //{
  //  if (claim == null)
  //  {
  //    return null;
  //  }
  //  return FromClaim(claim.Value, prefixToRemove);
  //}

  ///// <summary>
  ///// Create a permission from a claim
  ///// </summary>
  //public static Permission FromClaim(string claimValue, string prefixToRemove = null)
  //{
  //  Permission permission = new Permission();
  //  string[] valueParts = claimValue.Split(':');

  //  permission.Key = valueParts[0];
  //  permission.NormalizedKey = permission.Key;
  //  permission.Value = valueParts.Length > 1 ? valueParts[1] : null;

  //  if (!prefixToRemove.IsNullOrEmpty())
  //  {
  //    permission.NormalizedKey = valueParts[0].TrimStart(prefixToRemove);
  //  }

  //  return permission;
  //}
}