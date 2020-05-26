using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class SettingsGroup
  {
    public string Name { get; set; }

    public IList<SettingsArea> Items { get; set; } = new List<SettingsArea>();

    public SettingsGroup() { }

    public SettingsGroup(string name)
    {
      Name = name;
    }

    public void Add(string alias, string name, string description = null, string icon = null)
    {
      Items.Add(new SettingsArea(alias, name, description, icon));
    }

    public void AddInternal(string alias, string name, string description = null, string icon = null)
    {
      Items.Add(new InternalSettingsArea(alias, name, description, icon));
    }
  }
}
