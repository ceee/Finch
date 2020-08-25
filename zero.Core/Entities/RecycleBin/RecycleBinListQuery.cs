namespace zero.Core.Entities
{
  public class RecycleBinListQuery<T> : ListQuery<T> where T : IRecycledEntity
  {
    public string Group { get; set; }

    public string OperationId { get; set; }
  }
}
