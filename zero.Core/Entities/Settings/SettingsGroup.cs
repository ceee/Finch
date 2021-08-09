using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class InternalSettingsGroup : SettingsGroup { }

  public class SettingsGroup
  {
    public string Name { get; set; }

    public IList<SettingsArea> Items { get; set; } = new List<SettingsArea>();

    public SettingsGroup() { }

    public SettingsGroup(string name)
    {
      Name = name;
    }

    public void Add(string alias, string name, string description = null, string icon = null, string customUrl = null)
    {
      Items.Add(new SettingsArea(alias, name, description, icon, customUrl));
    }

    public void AddInternal(string alias, string name, string description = null, string icon = null)
    {
      Items.Add(new InternalSettingsArea(alias, name, description, icon));
    }
  }
}
