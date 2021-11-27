using Raven.Client.Documents;

namespace zero.Backoffice.Modules.Search;

public class zero_Backoffice_Search : ZeroJavascriptIndex
{
  public override void Setup(IZeroOptions options, IDocumentStore store)
  {
    // TODO index.Conventions is null, but needed for collection name retrieval

    foreach (var map in options.For<SearchOptions>())
    {
      Maps.Add(map.BuildInstruction(store));
    }


    //Index(nameof(SearchIndexResult.Name), FieldIndexing.Search);
    //Index(nameof(SearchIndexResult.Fields), FieldIndexing.Search);
  }
}