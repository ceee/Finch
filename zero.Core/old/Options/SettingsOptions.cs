using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Options
{
  public class SettingsOptions : ZeroBackofficeCollection<SettingsGroup>, IZeroCollectionOptions
  {
    public SettingsOptions()
    {
      
    }


    public void AddGroup<T>(int index = -1) where T : SettingsGroup, new()
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


    public void AddGroup(SettingsGroup group, int index = -1)
    {
      if (index > -1 && index < Items.Count)
      {
        Items.Insert(index, group);
      }
      else
      {
        Items.Add(group);
      }
    }


    public void AddToDefaultGroup(SettingsArea item, int index = -1)
    {
      SettingsGroup group = Items.FirstOrDefault(x => x.Name == "@settings.groups.system");

      if (group == null)
      {
        return;
      }

      if (index > -1 && index < group.Items.Count)
      {
        group.Items.Insert(index, item);
      }
      else
      {
        group.Items.Add(item);
      }
    }
  }
}
