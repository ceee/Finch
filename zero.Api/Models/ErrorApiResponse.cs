namespace zero.Api.Models;

public class ErrorApiResponse : ApiResponse
{
  public ErrorApiResponseError Error { get; set; }
}

public class ErrorApiResponseError
{
  public string Code { get; set; }

  public string Message { get; set; }

  public string ApiPath { get; set; }
}