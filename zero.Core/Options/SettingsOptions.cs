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
      // TODO find default group
      //if (index > -1 && index < Items.Count)
      //{
      //  Items.Insert(index, item);
      //}
      //else
      //{
      //  Items.Add(item);
      //}
    }
  }
}
