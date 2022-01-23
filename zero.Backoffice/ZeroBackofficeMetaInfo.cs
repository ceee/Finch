using System.Text.Json.Serialization;

namespace zero.Backoffice;

internal class ZeroBackofficeMetaInfo
{
  [JsonPropertyName("zeroVersion")]
  public string ZeroVersion { get; set; }

  [JsonPropertyName("appVersion")]
  public string AppVersion { get; set; }

  [JsonPropertyName("appLastModifiedDate")]
  public DateTimeOffset AppLastModifiedDate { get; set; }
}