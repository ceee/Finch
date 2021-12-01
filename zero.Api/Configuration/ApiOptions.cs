using zero.Api.Endpoints.Search;

namespace zero.Api.Configuration;

public class ApiOptions
{
  /// <summary>
  /// Configure search maps
  /// </summary>
  public SearchOptions Search { get; set; } = new();
}