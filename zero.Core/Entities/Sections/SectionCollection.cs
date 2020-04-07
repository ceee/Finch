using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class SectionCollection : List<ISection>
  {
    public void Add<T>() where T : ISection, new()
    {
      Add(new T());
    }

    public void Add(string alias, string name, string icon)
    {
      Add(new Section(alias, name, icon));
    }
  }
}
