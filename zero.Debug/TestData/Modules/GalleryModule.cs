using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.TestData
{
  public class GalleryModule : Module
  {
    public List<string> ImageIds { get; set; } = new List<string>();

    public bool IsBigger { get; set; }

    public bool IsFirstImageBigger { get; set; }
  }
}
