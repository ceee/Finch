using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.TestData
{
  public class TeamMemberSpace : Space
  {
    public string Reactor { get; set; }

    public TeamMemberSpace()
    {
      Reactor = "fine";
      Alias = "team";
      View = SpaceView.List;
      Name = "Team";
      Description = "Our team members";
      Icon = "fth-users";
      Type = typeof(TeamMember);
      AllowShared = true;
    }
  }

  //[Space("team", "@dbg.spaces.team.name", "fth-users", "@dbg.spaces.team.description")]
  public class TeamMember : SpaceContent
  {
    public string Position { get; set; }

    public Media Image { get; set; }

    public string Email { get; set; }

    public string VideoUri { get; set; }

    public List<TeamMemberAddress> Addresses { get; set; } = new List<TeamMemberAddress>();
  }


  public class TeamMemberAddress
  {
    public string City { get; set; }

    public string Street { get; set; }

    public string No { get; set; }

    public string CountryId { get; set; }
  }
}
