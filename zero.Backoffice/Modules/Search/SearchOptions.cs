namespace zero.Backoffice.Modules;

public class SearchOptions : OptionsEnumerable<SearchIndexMap>, IOptionsEnumerable
{
  public static string KEY { get; set; } = "Zero:Backoffice:Search";

  public bool Enabled { get; set; }


  public SearchOptions()
  {
    Enabled = true;
    //Map<Page>().Display((x, res, opts) =>
    //{
    //  PageType pageType = opts.Pages.GetByAlias(x.PageTypeAlias);
    //  if (pageType != null)
    //  {
    //    res.Icon = pageType.Icon;
    //  }
    //  res.Url = "/pages/edit/" + x.Id;
    //});
    //Map<MediaFolder>("fth-image");
  }


  public SearchIndexMap<T> Map<T>(string icon = null) where T : ZeroEntity, new()
  {
    SearchIndexMap<T> map = new(icon);
    Items.Add(map);
    return map;
  }
}