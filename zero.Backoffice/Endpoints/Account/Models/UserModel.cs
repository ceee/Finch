namespace zero.Backoffice.Endpoints.Account;

public class UserModel : ZeroIdEntity
{
  public string CurrentAppId { get; set; }

  public bool IsSuper { get; set; }

  public string AvatarId { get; set; }

  public string Email { get; set; }

  public string Name { get; set; }

  public bool IsActive { get; set; }

  public DateTimeOffset CreatedDate { get; set; }

  public string Flavor { get; set; }

  public string Culture { get; set; }
}
