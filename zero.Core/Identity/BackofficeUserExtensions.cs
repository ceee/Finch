namespace zero.Identity;

public static class BackofficeUserExtensions
{
  public static string[] GetAllowedAppIds(this ZeroUser user)
  {
    if (user == null)
    {
      return Array.Empty<string>();
    }


    return Array.Empty<string>();
    // TODO permissions
    //IEnumerable<Permission> permissions = user.Claims
    //  .Where(claim => claim.Type == Constants.Auth.Claims.Permission && claim.Value.StartsWith(Permissions.Applications))
    //  .Select(x => Permission.FromClaim(x.ToClaim(), Permissions.Applications));

    //return permissions.Where(x => x.IsTrue).Select(x => x.NormalizedKey).ToArray();
  }
}