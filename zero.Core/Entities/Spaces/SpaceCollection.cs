using System;
using System.Collections.Generic;
using System.Linq;

namespace zero.Core.Entities
{
  public class SpaceCollection : List<Space>
  {
    public Space GetByAlias(string alias)
    {
      return this.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    public void AddList<T>(string alias, string name, string description, string icon) where T : SpaceContent, new()
    {
      Add(new Space()
      {
        Alias = alias,
        View = SpaceView.List,
        Name = name,
        Description = description,
        Icon = icon,
        Type = typeof(T)
      });
    }


    public void AddEditor<T>(string alias, string name, string description, string icon) where T : SpaceContent, new()
    {
      Add(new Space()
      {
        Alias = alias,
        View = SpaceView.Editor,
        Name = name,
        Description = description,
        Icon = icon,
        Type = typeof(T)
      });
    }


    public void AddCustom<T>(string componentPath, string alias, string name, string description, string icon) where T : SpaceContent, new()
    {
      Add(new Space()
      {
        Alias = alias,
        View = SpaceView.Custom,
        ComponentPath = componentPath,
        Name = name,
        Description = description,
        Icon = icon,
        Type = typeof(T)
      });
    }


    public void AddCustom(string componentPath, string alias, string name, string description, string icon)
    {
      Add(new Space()
      {
        Alias = alias,
        View = SpaceView.Custom,
        ComponentPath = componentPath,
        Name = name,
        Description = description,
        Icon = icon
      });
    }
  }
}
