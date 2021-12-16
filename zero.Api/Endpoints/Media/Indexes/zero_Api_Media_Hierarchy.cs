using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Media;


public class MediaTreeHierarchyIndexResult : ZeroTreeHierarchyIndexResult
{
  public bool IsFolder { get; set; }
}

public class zero_Api_Media_Hierarchy : ZeroTreeHierarchyIndex<zero.Media.Media>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new MediaTreeHierarchyIndexResult
    {
      Id = item.Id,
      IsFolder = item.IsFolder,
      Path = Recurse(item, x => LoadDocument<zero.Media.Media>(x.ParentId))
        .Where(x => x != null && x.Id != null && x.Id != item.Id)
        .Reverse()
        .Select(current => current.Id)
        .ToList()
    });

    StoreAllFields(FieldStorage.Yes);
  }
}