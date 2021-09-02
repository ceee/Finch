using Raven.Client.Documents;
using zero.Core.Options;

namespace zero.Core.Database.Indexes
{
  public class Backoffice_Search : ZeroJavascriptIndex
  {
    public override void Setup(IZeroOptions options, IDocumentStore store)
    {
      // TODO index.Conventions is null, but needed for collection name retrieval

      foreach (var map in options.Search.GetAllItems())
      {
        Maps.Add(map.BuildInstruction(this, store));
      }


      //Index(nameof(SearchIndexResult.Name), FieldIndexing.Search);
      //Index(nameof(SearchIndexResult.Fields), FieldIndexing.Search);
    }
  }
}
