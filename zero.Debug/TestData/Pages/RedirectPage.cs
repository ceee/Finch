using System;
using zero.Core.Entities;

namespace zero.TestData
{
  public class RedirectPage : Page
  {
    public OptionsPagePartial Options { get; set; } = new OptionsPagePartial();

    public string Link { get; set; }
  }
}
