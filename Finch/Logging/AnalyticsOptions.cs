namespace Finch.Logging;

public class AnalyticsOptions
{
  public bool Enabled { get; set; } = true;

  public string Endpoint { get; set; } = "/api/hello";

  public string ProxyUrl { get; set; }

  public string ProxyScriptEndpoint { get; set; } = "/hi.js";

  public string ProxyTrackEndpoint { get; set; } = "/ping";

  public string TrackingId { get; set; }

  public bool Valid()
  {
    return Enabled && TrackingId.HasValue() && ProxyUrl.HasValue();
  }
}