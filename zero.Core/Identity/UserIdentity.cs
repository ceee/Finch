using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class UserIdentity : ClaimsIdentity
  {
    public const string Issuer = Constants.Auth.BackofficeScheme;

    public override string AuthenticationType => Issuer;


    public UserIdentity() { }

    private UserIdentity(ClaimsIdentity identity) : base(identity, identity.Claims, Issuer, Constants.Auth.Claims.UserName, Constants.Auth.Claims.Role)
    {

    }


    public static bool TryCreate(ClaimsIdentity identity, out UserIdentity user)
    {
      user = null;

      if (!RequiredClaims.All(claim => identity.HasClaim(x => x.Type == claim && !x.Value.IsNullOrWhiteSpace())))
      {        
        return false;
      }

      if (!identity.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True))
      {
        return false;
      }

      user = new UserIdentity(identity);
      return true;
    }


    public static IEnumerable<string> RequiredClaims => new[]
    {
      Constants.Auth.Claims.UserId,
      Constants.Auth.Claims.UserName,
      //Constants.Auth.Claims.Role,
      Constants.Auth.Claims.SecurityStamp,
      Constants.Auth.Claims.AppId,
      Constants.Auth.Claims.DefaultAppId,
      Constants.Auth.Claims.IsZero
    };
  }
}
