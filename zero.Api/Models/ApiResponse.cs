namespace zero.Api.Models
{
  public class ApiResponse
  {
    [JsonPropertyName("@metadata")]
    public ApiResponseMetadata Metadata { get; set; } = new();
  }
}
