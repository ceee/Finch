namespace zero.Core.Entities
{
  public class RecycleBinListQuery : ListQuery<IRecycledEntity>
  {
    public string Group { get; set; }

    public string OperationId { get; set; }
  }
}
