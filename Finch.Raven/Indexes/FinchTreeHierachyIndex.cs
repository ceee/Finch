using Raven.Client.Documents.Indexes;

namespace Finch.Raven;

public class FinchTreeHierarchyIndexResult : FinchIdEntity, ISupportsDbConventions
{
  public List<string> Path { get; set; } = new List<string>();
}

public abstract class FinchTreeHierarchyIndex<T> : FinchIndex<T, FinchTreeHierarchyIndexResult> where T : FinchIdEntity, ISupportsTrees
{
  protected override void Create()
  {
    Map = items => items.Select(item => new FinchTreeHierarchyIndexResult
    {
      Id = item.Id,
      Path = Recurse(item, x => LoadDocument<T>(x.ParentId))
        .Where(x => x != null && x.Id != null && x.Id != item.Id)
        .Reverse()
        .Select(current => current.Id)
        .ToList()
    });

    StoreAllFields(FieldStorage.Yes);
    //Index(x => x.ChannelId, FieldIndexing.Exact);
  }
}