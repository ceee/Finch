namespace zero.Backoffice.Endpoints.UI;

public class PreviewRequestTokenModel
{
  public string Key { get; set; }

  public class Response
  {
    public string Token { get; set; }

    public string QueryParameter { get; set; }
  }
}