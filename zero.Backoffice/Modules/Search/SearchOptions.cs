namespace zero.Backoffice.Modules;

public class SearchOptions : OptionsEnumerable<SearchIndexMap>, IOptionsEnumerable
{
  public bool Enabled { get; set; }

  public SearchIndexMap<T> Map<T>(string icon = null) where T : ZeroEntity, new()
  {
    SearchIndexMap<T> map = new(icon);
    Items.Add(map);
    return map;
  }
}