using System.Linq;
using zero.Core.Entities;

namespace zero.Configuration;

public class SpaceOptions : OptionsEnumerable<Space>, IOptionsEnumerable
{
  public SpaceOptions()
  {

  }


  public void Add<T>() where T : Space, new()
  {
    Items.Add(new T());
  }


  public void AddList<T>(string alias, string name, string description, string icon) where T : SpaceContent, new()
  {
    Items.Add(new Space()
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
    Items.Add(new Space()
    {
      Alias = alias,
      View = SpaceView.Editor,
      Name = name,
      Description = description,
      Icon = icon,
      Type = typeof(T)
    });
  }


  public void AddSeparator()
  {
    Space lastSpace = Items.LastOrDefault();

    if (lastSpace != null)
    {
      lastSpace.LineBelow = true;
    }
  }


  public void AddCustom<T>(string componentPath, string alias, string name, string description, string icon) where T : SpaceContent, new()
  {
    Items.Add(new Space()
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
    Items.Add(new Space()
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
