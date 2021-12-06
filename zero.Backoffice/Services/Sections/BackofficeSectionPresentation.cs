namespace zero.Backoffice.Services;

public class BackofficeSectionPresentation
{
  public string Alias { get; set; }

  public string Name { get; set; }

  public string Icon { get; set; }

  public string Color { get; set; }

  public string Url { get; set; }

  public bool IsExternal { get; set; }

  public IEnumerable<BackofficeSectionPresentation> Children { get; set; }
}