﻿using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Routing;

namespace zero.Core.Database.Indexes
{
  public class Routes_ForResolver : AbstractIndexCreationTask<Route>
  {
    public Routes_ForResolver()
    {
      Map = items => items
        .Select(x => new
        {
          Url = x.Url,
          AllowSuffix = x.AllowSuffix
        });
    }
  }
}