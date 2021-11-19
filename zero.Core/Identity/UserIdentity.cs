using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace zero;

public class BackofficeUserIdentity : UserIdentity
{
  protected BackofficeUserIdentity(ClaimsIdentity identity) : base(identity, Constants.Auth.BackofficeScheme) { }

  public static bool TryCreate(ClaimsIdentity identity, out BackofficeUserIdentity user)
  {
    user = null;

    if (!RequiredClaims.All(claim => identity.HasClaim(x => x.Type == claim && !x.Value.IsNullOrWhiteSpace())))
    {
      Console.Error.WriteLine("Could not find all claims for UserIdentity creation"); // TODO use logger
      return false;
    }
    if (!identity.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True))
    {
      Console.Error.WriteLine("Could not find all claims for UserIdentity creation"); // TODO use logger
      return false;
    }

    user = new BackofficeUserIdentity(identity);
    return true;
  }


  public static bool TryGet(IPrincipal principal, out BackofficeUserIdentity user)
  {
    if (TryGet(principal, out UserIdentity _user))
    {
      return TryCreate(_user, out user);
    }

    user = null;
    return false;
  }
}


public class UserIdentity : ClaimsIdentity
{
  protected UserIdentity(ClaimsIdentity identity, string authenticationType = null) : base(identity, identity.Claims, authenticationType ?? identity.AuthenticationType, Constants.Auth.Claims.UserName, Constants.Auth.Claims.Role)
  {

  }


  public static bool TryCreate(ClaimsIdentity identity, string authenticationType, out UserIdentity user)
  {
    user = null;

    if (!RequiredClaims.All(claim => identity.HasClaim(x => x.Type == claim && !x.Value.IsNullOrWhiteSpace())))
    {
      Console.Error.WriteLine("Could not find all claims for UserIdentity creation"); // TODO use logger
      return false;
    }

    user = new UserIdentity(identity, authenticationType);
    return true;
  }


  /// <summary>
  /// Build user identity from a matching principal identity
  /// </summary>
  public static bool TryGet(IPrincipal principal, out UserIdentity user)
  {
    if (principal.Identity is UserIdentity identity)
    {
      user = identity;
      return true;
    }

    if (principal is ClaimsPrincipal claimsPrincipal)
    {
      user = claimsPrincipal.Identities.OfType<UserIdentity>().FirstOrDefault();
      if (user != null)
      {
        return true;
      }
    }

    if (principal.Identity is ClaimsIdentity claimsIdentity && claimsIdentity.IsAuthenticated)
    {
      if (TryCreate(claimsIdentity, claimsIdentity.AuthenticationType, out user))
      {
        return true;
      }
    }

    user = null;
    return false;
  }


  public static IEnumerable<string> RequiredClaims => new[]
  {
    Constants.Auth.Claims.UserId,
    Constants.Auth.Claims.UserName,
    Constants.Auth.Claims.Email,
    //Constants.Auth.Claims.Role,
    Constants.Auth.Claims.SecurityStamp,
    Constants.Auth.Claims.IsZero
  };
}
