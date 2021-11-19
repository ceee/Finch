namespace zero.Core.Entities
{
  public class MediaListQuery : ListQuery<Media>
  {
    public string FolderId { get; set; }
  }


  public class MediaListItemQuery : ListQuery<MediaListItem>
  {
    public string FolderId { get; set; }

    public bool SearchIsGlobal { get; set; }
  }
}
