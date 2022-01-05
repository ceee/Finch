namespace zero.Pages;

public static class FlavorOptionsExtensions
{
  public static void AddPage<T>(this FlavorOptions options, string alias, string name, string description, string icon) where T : Page, new()
  {
    options.Add<Page, T>(new FlavorConfig(typeof(T))
    {
      Alias = alias,
      //EditorAlias = editorAlias.Or(alias),
      Construct = _ => new T(),
      Name = name,
      Description = description,
      Icon = icon
    });
  }
}
