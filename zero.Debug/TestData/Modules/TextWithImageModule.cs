using System;
using zero.Core.Entities;

namespace zero.TestData
{
  public class TextWithImageModule : Module
  {
    public string Headline { get; set; }

    public string Text { get; set; }

    public string ImageId { get; set; }

    public bool IsLeftAligned { get; set; }
  }
}
