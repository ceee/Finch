namespace zero.Api;

public struct ApiRequestHints
{
  public ApiResponsePreference ResponsePreference { get; set; }
}


/// <summary>
/// Preference for POST + PUT requests
/// see https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md#75-standard-request-headers
/// </summary>
public enum ApiResponsePreference
{
  /// <summary>
  /// Returns a minimal repsonse for inserts and updates
  /// </summary>
  Minimal = 0,
  /// <summary>
  /// Returns status as well as model for inserts and updates
  /// </summary>
  Representation = 1
}