using zero.Core.Identity;

namespace zero.Core.Security
{
  public class ZeroAuthOptions<TUser> where TUser : IIdentityUser
  {
    public string Scheme { get; set; }

    public string Path { get; set; } = "/";
  }
}
