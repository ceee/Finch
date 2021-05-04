using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Pages_AsHistory : AbstractIndexCreationTask<Page, Pages_AsHistory.Result>
  {
    public class Result : ZeroIdEntity, IZeroDbConventions
    {
      public DateTimeOffset LastModified { get; set; }
    }


    public Pages_AsHistory()
    {
      Map = items => items
        .Select(x => new Result()
        {
          Id = x.Id,
          LastModified = x.LastModifiedDate
        });

      StoreAllFields(FieldStorage.Yes);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
