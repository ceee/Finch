using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class SectionCollection : List<ISection>
  {
    public void Add<T>(int index = -1) where T : ISection, new()
    {
      if (index > -1 && index < Count)
      {
        Insert(index, new T());
      }
      else
      {
        Add(new T());
      }
    }

    public void Add(string alias, string name, string icon)
    {
      Add(new Section(alias, name, icon));
    }
  }
}
