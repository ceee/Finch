using zero.Core.Entities;

namespace zero.TestData
{
  public class TeamMember : ListItem
  {
    public string Position { get; set; }

    public Media Image { get; set; }

    public string Email { get; set; }

    public string VideoUri { get; set; }
  }
}
