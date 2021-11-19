using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace zero;

public class Permission
{
  /// <summary>
  /// Full key (stored in left part claim value) of the permission
  /// </summary>
  public string Key { get; set; }

  /// <summary>
  /// Normalized key with optionally removed prefix
  /// </summary>
  public string NormalizedKey { get; set; }

  /// <summary>
  /// Value of the permission (boolean, read/write, ...). Stored in right part of claim value
  /// </summary>
  public string Value { get; set; }

  /// <summary>
  /// Title of this permission for output
  /// </summary>
  public string Label { get; set; }

  /// <summary>
  /// Optional description text
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Datatype of the value
  /// </summary>
  public PermissionValueType ValueType { get; set; }

  /// <summary>
  /// Renders a custom component for this permission in the backoffice
  /// </summary>
  public string CustomComponentPath { get; set; }


  public Permission() { }

  public Permission(string key, string value, PermissionValueType valueType = PermissionValueType.CRUD)
  {
    Key = key;
    Value = value;
    ValueType = valueType;
  }

  public Permission(string key, string label, string description, PermissionValueType valueType = PermissionValueType.CRUD)
  {
    Key = key;
    Label = label;
    Description = description;
    ValueType = valueType;
  }


  /// <summary>
  /// Whether the value is read or write
  /// </summary>
  public bool CanRead => Value == PermissionsValue.Read || Value == PermissionsValue.Update;

  /// <summary>
  /// Whether the value is write
  /// </summary>
  public bool CanWrite => Value == PermissionsValue.Update;

  /// <summary>
  /// Whether the value is true
  /// </summary>
  public bool IsTrue => Value == PermissionsValue.True;

  /// <summary>
  /// Whether the value is false
  /// </summary>
  public bool IsFalse => Value == PermissionsValue.False;

  /// <summary>
  /// Whether a resource (with authorization based on the key) can be read by any of the given permisssions
  /// </summary>
  public static bool CanReadKey(IEnumerable<Permission> permissions, string key, bool isNormalized = false)
  {
    Permission permission = permissions.FirstOrDefault(p => isNormalized ? p.NormalizedKey == key : p.Key == key);
    return permission?.CanRead ?? false;
  }

  /// <summary>
  /// Whether a resource (with authorization based on the key) can be written to by any of the given permisssions
  /// </summary>
  public static bool CanWriteKey(IEnumerable<Permission> permissions, string key, bool isNormalized = false)
  {
    Permission permission = permissions.FirstOrDefault(p => isNormalized ? p.NormalizedKey == key : p.Key == key);
    return permission?.CanWrite ?? false;
  }

  /// <summary>
  /// Create a permission from a claim
  /// </summary>
  public static Permission FromClaim(Claim claim, string prefixToRemove = null)
  {
    if (claim == null)
    {
      return null;
    }
    return FromClaim(claim.Value, prefixToRemove);
  }

  /// <summary>
  /// Create a permission from a claim
  /// </summary>
  public static Permission FromClaim(string claimValue, string prefixToRemove = null)
  {
    Permission permission = new Permission();
    string[] valueParts = claimValue.Split(':');

    permission.Key = valueParts[0];
    permission.NormalizedKey = permission.Key;
    permission.Value = valueParts.Length > 1 ? valueParts[1] : null;

    if (!prefixToRemove.IsNullOrEmpty())
    {
      permission.NormalizedKey = valueParts[0].TrimStart(prefixToRemove);
    }

    return permission;
  }
}