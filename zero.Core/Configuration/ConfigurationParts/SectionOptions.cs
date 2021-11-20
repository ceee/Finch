using zero.Core.Entities;

namespace zero.Configuration;

public class SectionOptions : OptionsEnumerable<ISection>, IOptionsEnumerable
{
  public SectionOptions()
  {
      
  }


  public void Add<T>(int index = -1) where T : ISection, new()
  {
    if (index > -1 && index < Items.Count)
    {
      Items.Insert(index, new T());
    }
    else
    {
      Items.Add(new T());
    }
  }

  public void Add(string alias, string name, string icon)
  {
    Items.Add(new Section(alias, name, icon));
  }
}
