using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.TestData
{
  public class TeamMember : ListItem
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
