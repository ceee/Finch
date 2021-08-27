using Raven.Client.Documents.Indexes;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Backoffice_Search : ZeroMultiMapIndex
  {
    public class Result
    {
      public string Id { get; set; }

      public string Group { get; set; }

      public string Name { get; set; }

      public bool IsActive { get; set; }

      public Dictionary<string, string> Fields { get; set; } = new();
    }


    protected override void Create()
    {
      AddEntities();
      Index(nameof(Result.Name), FieldIndexing.Search);
      Index(nameof(Result.Fields), FieldIndexing.Search);
    }


    protected virtual void AddEntities()
    {
      AddPages();
      AddMediaFolders();
      //AddMedia();
    }


    protected virtual void AddPages()
    {
      AddMap<Page>(map => map.Select(page => new Result()
      {
        Id = page.Id,
        Group = "zero.page",
        Name = page.Name,
        IsActive = page.IsActive,
        Fields = new()
      }));
    }


    protected virtual void AddMediaFolders()
    {
      AddMap<MediaFolder>(map => map.Select(page => new Result()
      {
        Id = page.Id,
        Group = "zero.mediafolder",
        Name = page.Name,
        IsActive = page.IsActive,
        Fields = new()
      }));
    }


    protected virtual void AddMedia()
    {
      AddMap<Media>(map => map.Select(page => new Result()
      {
        Id = page.Id,
        Group = "zero.media",
        Name = page.Name,
        IsActive = page.IsActive,
        Fields = new()
      }));
    }
  }
}
