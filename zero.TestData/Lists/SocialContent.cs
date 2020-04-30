using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.TestData
{
  public class SocialContent : SpaceContent
  {
    public bool IsVisible { get; set; }

    public string Twitter { get; set; }

    public string Facebook { get; set; }

    public string Youtube { get; set; }

    public string xRte { get; set; }

    public string xTextarea { get; set; }

    public string xState { get; set; }

    public string xMedia { get; set; }

    public string xIconPicker { get; set; }

    public List<SocialAddress> Addresses { get; set; } = new List<SocialAddress>();
  }

  public class SocialAddress
  {
    public string City { get; set; }

    public string Street { get; set; }

    public string No { get; set; }

    public string CountryId { get; set; }
  }
}
