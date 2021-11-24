namespace zero.Backoffice.UIComposition;

public class BackofficeSettingsOptions : List<SettingsGroup>
{
  public void AddGroup<T>(int index = -1) where T : SettingsGroup, new()
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


  public void AddGroup(SettingsGroup group, int index = -1)
  {
    if (index > -1 && index < Count)
    {
      Insert(index, group);
    }
    else
    {
      Add(group);
    }
  }


  public void AddToDefaultGroup(SettingsArea item, int index = -1)
  {
    SettingsGroup group = this.FirstOrDefault(x => x.Name == "@settings.groups.system");

    if (group == null)
    {
      return;
    }

    if (index > -1 && index < group.Areas.Count)
    {
      group.Areas.Insert(index, item);
    }
    else
    {
      group.Areas.Add(item);
    }
  }
}
