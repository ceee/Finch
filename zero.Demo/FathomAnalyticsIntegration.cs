using zero.Configuration;

namespace zero.Demo
{
  public class FathomAnalyticsIntegration : IntegrationModel
  {
    /// <summary>
    /// ID of the site in Fathom
    /// </summary>
    public string SiteId { get; set; }

    /// <summary>
    /// Custom domain URL (without /script.js)
    /// </summary>
    public string CustomDomain { get; set; }
  }
}