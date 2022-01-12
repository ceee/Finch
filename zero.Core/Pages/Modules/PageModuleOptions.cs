namespace zero.Pages;

public class PageModuleOptions : List<PageModuleType>
{
  public void Add<T>(string alias, string name, string description, string icon, string group = null, List<string> tags = null, List<string> disallowedPageTypes = null) where T : PageModule, new()
  {
    Add(new PageModuleType(typeof(T))
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon,
      Group = group,
      Tags = tags ?? new(),
      DisallowedPageTypes = disallowedPageTypes ?? new(),
      Construct = _ => new T()
    });
  }
}