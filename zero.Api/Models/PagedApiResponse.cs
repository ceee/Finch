namespace zero.Api.Models;

public class PagedApiResponse : ApiResponse
{
  public object Items { get; set; }
  public PagedApiResponsePaging Paging { get; set; }
}


public class PagedApiResponsePaging
{
  public long Page { get; set; }

  public long PageSize { get; set; }

  public long TotalPages { get; set; }

  public long TotalItems { get; set; }

  public bool HasMore { get; set; }
}