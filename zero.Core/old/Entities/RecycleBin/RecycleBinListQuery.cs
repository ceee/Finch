namespace zero.Core.Entities
{
  public class RecycleBinListQuery : ListQuery<RecycledEntity>
  {
    public string Group { get; set; }

    public string OperationId { get; set; }
  }
}
