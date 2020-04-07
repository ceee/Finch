using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core
{
  public class ZeroOptions
  {
    public string BackofficePath { get; set; }

    public SectionCollection Sections { get; private set; } = new SectionCollection();

    public IList<SettingsGroup> SettingsAreas { get; private set; } = new List<SettingsGroup>();
  }
}
