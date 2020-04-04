using zero.Core.Entities.Sections;

namespace zero.Core
{
  public class ZeroOptions
  {
    public SectionCollection Sections { get; private set; } = new SectionCollection();
  }
}
