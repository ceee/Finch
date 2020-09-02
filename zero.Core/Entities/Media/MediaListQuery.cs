namespace zero.Core.Entities
{
  public class MediaListQuery : ListQuery<IMedia>
  {
    public string FolderId { get; set; }
  }


  public class MediaListItemQuery : ListQuery<MediaListItem>
  {
    public string FolderId { get; set; }
  }
}
