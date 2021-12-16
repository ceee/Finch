namespace zero.Api.Endpoints.Media;

public class MediaBulkMoveOperation
{
  public string ParentId { get; set; }

  public string[] Ids { get; set; }
}