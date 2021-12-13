namespace zero.Api.Models;

public class PagedDataApiResponse : DataApiResponse
{
  public long Page { get; set; }

  public long PageSize { get; set; }

  public long TotalPages { get; set; }

  public long TotalItems { get; set; }
}
