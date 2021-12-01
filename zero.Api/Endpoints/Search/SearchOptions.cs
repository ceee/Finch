namespace zero.Api.Endpoints.Search;

public class SearchOptions : List<SearchIndexMap>
{
  public bool Enabled { get; set; }

  public SearchIndexMap<T> Map<T>(string icon = null) where T : ZeroEntity, new()
  {
    SearchIndexMap<T> map = new(icon);
    Add(map);
    return map;
  }
}