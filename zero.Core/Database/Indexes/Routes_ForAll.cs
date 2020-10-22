using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Routing;

namespace zero.Core.Database.Indexes
{
  public class Routes_ForAll : AbstractMultiMapIndexCreationTask<Route>
  {
    public Routes_ForAll()
    {
      AddMapForAll<IRoutedEntity>(items => items.Where(x => x.Route != null).Select(item => new Route()
      {
        Id = item.Id,
        AppId = item.AppId,
        Url = item.Route.Url,
        Dependencies = item.Route.Dependencies,
        ProviderAlias = MetadataFor(item)["@collection"].ToString()
      }));

      StoreAllFields(FieldStorage.Yes);
      Index(x => x.Url, FieldIndexing.Exact);
      Index(x => x.AppId, FieldIndexing.Exact);
      Index(x => x.Dependencies, FieldIndexing.Exact);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
