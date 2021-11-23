using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;

namespace zero.Backoffice.Modules;

public class BackofficeSearchService : IBackofficeSearchService
{
  protected IZeroStore Store { get; private set; }

  protected IZeroOptions Options { get; private set; }
    

  public BackofficeSearchService(IZeroStore store, IZeroOptions options)
  {
    Store = store;
    Options = options;
  }


  public async Task<Paged<SearchResult>> Query(string searchTerm)
  {
    string[] searchParts = searchTerm.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
    {
      return "*" + x + "*";
    }).ToArray();

    List<ZeroEntity> results = await Store.Session().Query<SearchIndexResult, zero_Backoffice_Search>()
      .Statistics(out QueryStatistics stats)
      .Search(x => x.Name, searchParts, 2, @operator: SearchOperator.And)
      .Search(x => x.Fields, searchParts, 1, Raven.Client.Documents.SearchOptions.Or, @operator: SearchOperator.And)
      .Paging(1, 20)
      .As<ZeroEntity>()
      .ToListAsync();

    List<SearchResult> items = new();

    IReadOnlyCollection<SearchIndexMap> maps = Options.For<SearchOptions>().GetAllItems();

    foreach (ZeroEntity result in results)
    {
      Type type = result.GetType();
      SearchIndexMap map = maps.FirstOrDefault(x => x.CanModify(type));

      SearchResult searchResult = new()
      {
        Id = result.Id,
        Icon = map._Icon.Or("fth-folder"),
        Name = result.Name,
        IsActive = result.IsActive,
        Group = GetGroupName(map.Type),
        Url = "/" 
      };

      await map.Modify(result, searchResult, Options);

      items.Add(searchResult);
    }

    return new Paged<SearchResult>(items, stats.TotalResults, 1, 20);
  }


  protected string GetGroupName(Type type)
  {
    return "@search.collection." + type.Name.ToCamelCase();
  }
}

public interface IBackofficeSearchService
{
  Task<Paged<SearchResult>> Query(string searchTerm);
}