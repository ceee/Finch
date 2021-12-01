using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Mails;

public class zero_Api_MailTemplates_Listing : ZeroIndex<MailTemplate>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      item.Name,
      item.Key,
      item.Subject,
      item.CreatedDate
    });

    Index(x => x.Name, FieldIndexing.Search);
    Index(x => x.Key, FieldIndexing.Search);
    Index(x => x.Subject, FieldIndexing.Search);
  }
}