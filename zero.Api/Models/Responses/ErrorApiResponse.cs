namespace zero.Api.Models;

public class ErrorApiResponse : ApiResponse
{
  public List<ErrorApiResponseError> Errors { get; set; } = new();
}

public class ErrorApiResponseError
{
  public string Code { get; set; }

  public string Category { get; set; }

  public string Message { get; set; }

  public string Property { get; set; }
}