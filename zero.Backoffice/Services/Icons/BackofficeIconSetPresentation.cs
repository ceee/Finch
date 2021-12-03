namespace zero.Backoffice.Services;

public class BackofficeIconSetPresentation
{
  public string Alias { get; set; }

  public string Name { get; set; }

  public string Prefix { get; set; }

  public IEnumerable<string> Icons { get; set; }
}