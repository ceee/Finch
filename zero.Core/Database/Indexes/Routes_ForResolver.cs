﻿using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Routing;

namespace zero.Core.Database.Indexes
{
  public class Routes_ForResolver : ZeroIndex<Route>
  {
    protected override void Create()
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