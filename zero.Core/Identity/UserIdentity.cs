using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class UserIdentity : ClaimsIdentity
  {
    public const string Issuer = Constants.Auth.Scheme;

    public override string AuthenticationType => Issuer;


    public UserIdentity() { }

    private UserIdentity(ClaimsIdentity identity) : base(identity.Claims, Issuer)
    {

    }


    public static bool TryCreate(ClaimsIdentity identity, out UserIdentity user)
    {
      user = null;

      if (!RequiredClaims.All(claim => identity.HasClaim(x => x.Type == claim && !x.Value.IsNullOrWhiteSpace())))
      {        
        return false;
      }

      if (!identity.HasClaim(Constants.Auth.Claims.IsSuper, PermissionsValue.True))
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
      Constants.Auth.Claims.Role,
      Constants.Auth.Claims.SecurityStamp,
      Constants.Auth.Claims.CurrentAppId,
      Constants.Auth.Claims.DefaultAppId,
      Constants.Auth.Claims.IsZero
    };
  }
}
