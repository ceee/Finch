namespace zero.Api.Endpoints.Users;

public class UserBasic : ZeroIdEntity
{
  public string Name { get; set; }

  public bool IsActive { get; set; }

  public string Email { get; set; }

  public string AvatarId { get; set; }
}