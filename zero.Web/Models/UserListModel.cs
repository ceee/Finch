namespace zero.Web.Models
{
  public class UserListModel : ListModel
  {
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public string Email { get; set; }

    public string Roles { get; set; }

    public string Avatar { get; set; }
  }
}
