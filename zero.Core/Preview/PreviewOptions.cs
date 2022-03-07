namespace zero.Routing;

public class PreviewOptions
{
  public PreviewOptions()
  {
    Path = "/preview";
    QueryParameter = "zero_preview";
    TokenExpirationInMinutes = 10;
  }

  public string Path { get; set; }

  public string QueryParameter { get; set; }

  public double TokenExpirationInMinutes { get; set; }
}