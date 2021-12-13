namespace zero.Api.Models
{
  public class ApiResponse
  {
    [JsonPropertyOrder(-2)]
    public bool Success { get; set; } 

    [JsonPropertyOrder(-1)]
    public int Status { get; set; }

    public ApiResponseMetadata Metadata { get; set; } = new();
  }
}
