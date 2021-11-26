namespace zero.Spaces;

public class SpaceOptions : List<SpaceType>
{
  public void Add<T>() where T : SpaceType, new()
  {
    Add(new T());
  }


  public void AddList<T>(string alias, string name, string description, string icon) where T : Space, new()
  {
    Add(new SpaceType()
    {
      Alias = alias,
      View = SpaceView.List,
      Name = name,
      Description = description,
      Icon = icon,
      Type = typeof(T)
    });
  }


  public void AddEditor<T>(string alias, string name, string description, string icon) where T : Space, new()
  {
    Add(new SpaceType()
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
    SpaceType lastSpace = this.LastOrDefault();

    if (lastSpace != null)
    {
      lastSpace.LineBelow = true;
    }
  }


  public void AddCustom<T>(string componentPath, string alias, string name, string description, string icon) where T : Space, new()
  {
    Add(new SpaceType()
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
    Add(new SpaceType()
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
