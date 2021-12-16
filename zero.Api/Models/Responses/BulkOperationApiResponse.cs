namespace zero.Api.Models;


public class BulkOperationApiResponse : ApiResponse
{
  public int CountSucceeded { get; set; }

  public int CountFailed { get; set; }
}

public class BulkOperationWithErrorsApiResponse : BulkOperationApiResponse
{
  public List<BulkOperationErrorApiResponseError> Errors { get; set; } = new();
}

public class BulkOperationErrorApiResponseError : ErrorApiResponseError
{
  public string AffectedId { get; set; }
}