using zero.Core.Entities;

namespace zero.Core.Security
{
  public class ZeroAuthOptions<TUser> where TUser : IIdentityUser
  {
    public string Scheme { get; set; }

    public string Path { get; set; } = "/";
  }
}
