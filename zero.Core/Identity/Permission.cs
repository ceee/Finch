using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class Permission
  {
    public string Key { get; set; }

    public string NormalizedKey { get; set; }

    public string Value { get; set; }

    public bool CanRead => Value == PermissionsValue.Read || Value == PermissionsValue.Write;

    public bool CanWrite => Value == PermissionsValue.Write;

    public bool IsTrue => Value == PermissionsValue.True;

    public bool IsFalse => Value == PermissionsValue.False;


    public Permission() { }

    public Permission(Claim claim, string prefixToRemove = null) : this(claim.Value, prefixToRemove) { }

    public Permission(string claimValue, string prefixToRemove = null)
    {
      string[] valueParts = claimValue.Split(':');

      Key = valueParts[0];
      NormalizedKey = Key;
      Value = valueParts.Length > 1 ? valueParts[1] : null;

      if (!prefixToRemove.IsNullOrEmpty())
      {
        NormalizedKey = valueParts[0].TrimStart(prefixToRemove);
      }
    }


    public static bool CanReadKey(IEnumerable<Permission> permissions, string key, bool isNormalized = false)
    {
      Permission permission = permissions.FirstOrDefault(p => isNormalized ? p.NormalizedKey == key : p.Key == key);

      if (permission == null)
      {
        return false;
      }

      return permission.CanRead;
    }


    public static bool CanWriteKey(IEnumerable<Permission> permissions, string key, bool isNormalized = false)
    {
      Permission permission = permissions.FirstOrDefault(p => isNormalized ? p.NormalizedKey == key : p.Key == key);

      if (permission == null)
      {
        return false;
      }

      return permission.CanWrite;
    }
  }
}
