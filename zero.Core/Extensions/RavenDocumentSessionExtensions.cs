using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Extensions
{
  public static class RavenDocumentSessionExtensions
  {
    /// <summary>
    /// Get revision list (without content JSON) for an entity
    /// </summary>
    public static async Task<ListResult<Revision>> GetRevisions<T>(this IAsyncDocumentSession session, string id, int pageNumber = 1, int pageSize = 30) where T : IZeroEntity
    {
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
          Date = date.HasValue ? new DateTimeOffset(date.Value) : item.CreatedDate
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
}
