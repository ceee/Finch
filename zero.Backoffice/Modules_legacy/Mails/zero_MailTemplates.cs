using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class zero_MailTemplates : ZeroIndex<MailTemplate>
  {
    protected override void Create()
    {
      Map = items => items.Select(item => new
      {
        Name = item.Name,
        Key = item.Key,
        Subject = item.Subject,
        CreatedDate = item.CreatedDate
      });

      Index(x => x.Name, FieldIndexing.Search);
      Index(x => x.Key, FieldIndexing.Search);
      Index(x => x.Subject, FieldIndexing.Search);
    }
  }
}
