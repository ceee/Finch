namespace zero;

public class ZeroAuthOptions<TUser> where TUser : ZeroIdentityUser
{
  public string Scheme { get; set; }

  public string Path { get; set; } = "/";
}
