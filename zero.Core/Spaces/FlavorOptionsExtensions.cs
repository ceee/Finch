namespace zero.Spaces;

public static class FlavorOptionsExtensions
{
  public static void AddSpaceList<T>(this FlavorOptions options, string alias, string name, string description, string icon, string editorAlias = null) where T : Space, new()
  {
    options.Add<Space, T>(new SpaceType(typeof(T))
    {
      Alias = alias,
      EditorAlias = editorAlias.Or(alias),
      View = SpaceView.List,
      Construct = _ => new T(),
      Name = name,
      Description = description,
      Icon = icon
    });
  }


  public static void AddSpaceEditor<T>(this FlavorOptions options, string alias, string name, string description, string icon, string editorAlias = null) where T : Space, new()
  {
    options.Add<Space, T>(new SpaceType(typeof(T))
    {
      Alias = alias,
      EditorAlias = editorAlias.Or(alias),
      View = SpaceView.Editor,
      Construct = _ => new T(),
      Name = name,
      Description = description,
      Icon = icon
    });
  }

  public static void AddSpaceCustom<T>(this FlavorOptions options, string componentPath, string alias, string name, string description, string icon, string editorAlias = null) where T : Space, new()
  {
    options.Add<Space, T>(new SpaceType(typeof(T))
    {
      Alias = alias,
      EditorAlias = editorAlias.Or(alias),
      View = SpaceView.Custom,
      Construct = _ => new T(),
      ComponentPath = componentPath,
      Name = name,
      Description = description,
      Icon = icon
    });
  }
}
