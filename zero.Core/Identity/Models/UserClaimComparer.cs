namespace zero.Identity;

public class UserClaimComparer : IEqualityComparer<UserClaim>
{
  public bool Equals(UserClaim x, UserClaim y)
  {
    return (x == null && y == null) || (x.Type.Equals(y.Type, StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(y.Value, StringComparison.InvariantCultureIgnoreCase));
  }

  public int GetHashCode(UserClaim obj)
  {
    return (obj.Type + obj.Value).GetHashCode();
  }
}
