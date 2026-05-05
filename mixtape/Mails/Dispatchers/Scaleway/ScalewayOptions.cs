namespace Mixtape.Mails.Dispatchers.Scaleway;

public class ScalewayOptions
{
  public string ApiUrl { get; set; } = "https://api.scaleway.com";

  public string ProjectId { get; set; }

  public string SecretKey { get; set; }

  public string Region { get; set; } = "fr-par";
}