using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace zero.Raven;

public class ZeroTreeHierarchyIndexResult : ZeroIdEntity, ISupportsDbConventions
{
  public List<string> Path { get; set; } = new List<string>();
}

public abstract class ZeroTreeHierarchyIndex<T> : ZeroIndex<T, ZeroTreeHierarchyIndexResult> where T : ZeroIdEntity, ISupportsTrees
{
  protected override void Create()
  {
    Map = items => items.Select(item => new ZeroTreeHierarchyIndexResult
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