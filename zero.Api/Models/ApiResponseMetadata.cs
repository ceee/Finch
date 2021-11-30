namespace zero.Api.Models;

public class ApiResponseMetadata
{
  public DateTimeOffset RequestDate { get; set; }

  public TimeSpan Duration { get; set; }
}