using System;
using zero.Core.Entities;

namespace zero.TestData
{
  public class ContentPage : Page
  {
    public MetaPagePartial Meta { get; set; } = new MetaPagePartial();

    public OptionsPagePartial Options { get; set; } = new OptionsPagePartial();

    public string Text { get; set; }
  }
}
