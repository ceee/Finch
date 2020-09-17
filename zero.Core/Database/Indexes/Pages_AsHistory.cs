using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Pages_AsHistory : AbstractIndexCreationTask<IPage, Pages_AsHistory.Result>
  {
    public class Result : IZeroIdEntity, IAppAwareEntity, IZeroDbConventions
    {
      public string Id { get; set; }

      public string AppId { get; set; }

      public DateTimeOffset LastModified { get; set; }
    }


    public Pages_AsHistory()
    {
      Map = items => items
        .Select(x => new Result()
        {
          Id = x.Id,
          AppId = x.AppId,
          LastModified = x.LastModifiedDate
        });

      StoreAllFields(FieldStorage.Yes);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
