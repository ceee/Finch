namespace zero.Backoffice.Modules;

public class SearchResult
{
  public string Id { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public string Icon { get; set; }

  public string Image { get; set; }

  public string Url { get; set; }

  public string Group { get; set; }

  public bool IsActive { get; set; }
}


public class SearchIndexResult
{
  public string Id { get; set; }

  public string Group { get; set; }

  public string Name { get; set; }

  public bool IsActive { get; set; }

  public List<string> Fields { get; set; } = new();
}