using zero.Configuration;

namespace zero.Demo
{
  public class GoogleAnalyticsIntegration : IntegrationModel
  {
    /// <summary>
    /// Provided tracking ID from Google
    /// </summary>
    public string TrackingId { get; set; }

    /// <summary>
    /// Verifying ownership of the site (via Google Search Console) with this id
    /// Found in verification method -> HTML Tag
    /// </summary>
    public string SiteVerificationId { get; set; }
  }
}