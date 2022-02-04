using Raven.Client.Documents;

namespace zero.Search;

public class zero_Search : ZeroJavascriptIndex
{
  public override void Setup(IZeroOptions options, IDocumentStore store)
  {
    // TODO index.Conventions is null, but needed for collection name retrieval

    foreach (var map in options.For<ZeroSearchOptions>())
    {
      Maps.Add(map.BuildInstruction(store));
    }


    //Index(nameof(SearchIndexResult.Name), FieldIndexing.Search);
    //Index(nameof(SearchIndexResult.Fields), FieldIndexing.Search);
  }
}