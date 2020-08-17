using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class RevisionsApi : IRevisionsApi
  {
    protected IZeroOptions Options { get; set; }

    protected IDocumentStore Raven { get; set; }


    public RevisionsApi(IZeroOptions options, IDocumentStore raven)
    {
      Options = options;
      Raven = raven;
    }


    /// <summary>
    /// Get revision list for an entity
    /// </summary>
    public async Task<ListResult<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 30, bool includeContent = false) where T : IZeroEntity
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      List<T> items = await session.Advanced.Revisions.GetForAsync<T>(id, pageNumber - 1, pageSize);
      List<Revision> revisions = new List<Revision>();

      string[] userIds = items.Select(x => x.LastModifiedById).Distinct().ToArray();
      Dictionary<string, User> users = await session.LoadAsync<User>(userIds);

      foreach (T item in items)
      {
        DateTime? date = session.Advanced.GetLastModifiedFor(item);

        Revision revision = new Revision()
        {
          ChangeVector = session.Advanced.GetChangeVectorFor(item),
          Date = date.HasValue ? new DateTimeOffset(date.Value) : item.CreatedDate,
          Json = includeContent ? JsonConvert.SerializeObject(item) : null
        };

        if (!item.LastModifiedById.IsNullOrEmpty() && users.TryGetValue(item.LastModifiedById, out User user))
        {
          revision.User = new RevisionUser()
          {
            AvatarId = user.AvatarId,
            Id = user.Id,
            Name = user.Name
          };
        }

        revisions.Add(revision);
      }

      return new ListResult<Revision>(revisions, revisions.Count, pageNumber, pageSize);
    }
  }


  public interface IRevisionsApi
  {
    /// <summary>
    /// Get revision list (without content JSON) for an entity
    /// </summary>
    Task<ListResult<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 30, bool includeContent = false) where T : IZeroEntity;
  }
}
