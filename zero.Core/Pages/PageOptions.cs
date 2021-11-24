namespace zero.Pages;

public class PageOptions : List<PageType>
{
  public string Root { get; set; } = Constants.Pages.DefaultRootPageTypeAlias;


  public void Add<T>(PageType<T> pageType) where T : Page, new()
  {
    Add(PageType.Convert(pageType));
  }


  public void Add<T>(string alias, string name, string description, string icon) where T : Page, new()
  {
    Add(new PageType(typeof(T))
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon
    });
  }


  public void Add(Type type, string alias, string name, string description, string icon)
  {
    Add(new PageType(type)
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon
    });
  }

  public PageType GetByAlias(string alias)
  {
    return this.FirstOrDefault(x => x.Alias == alias);
  }
}
