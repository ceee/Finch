namespace zero.Api.Models;

public class PagedDataApiResponse : DataApiResponse
{
  public PagedDataApiResponsePaging Paging { get; set; }

  public Dictionary<string, string> Properties { get; set; } = new();
}


public class PagedDataApiResponsePaging
{
  public long Page { get; set; }

  public long PageSize { get; set; }

  public long TotalPages { get; set; }

  public long TotalItems { get; set; }
}