using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class RevisionsApi : IRevisionsApi
  {
    protected IAsyncDocumentSession Session { get; set; }

    public RevisionsApi(IAsyncDocumentSession session)
    {
      Session = session;
    }


    /// <summary>
    /// Get revision list for an entity
    /// </summary>
    public async Task<ListResult<Revision<T>>> GetPagedWithModel<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity
    {
      // get paged revisions
      List<T> items = await Session.Advanced.Revisions.GetForAsync<T>(id, (pageNumber - 1) * pageSize, pageSize);
      long totalResults = await Session.Advanced.Revisions.GetCountForAsync(id);

      // add creation for last page
      //if (!items.Any() && pageNumber >= (int)Math.Ceiling((decimal)totalResults / pageSize))
      //{
      //  T currentRevision = await Session.LoadAsync<T>(id);
      //  if (currentRevision != null)
      //  {
      //    items.Add(currentRevision);
      //  }
      //}

      List<Revision<T>> revisions = new();

      // load affected users as the revisions could have been edited by other users too
      string[] userIds = items.Select(x => x.LastModifiedById).Distinct().ToArray();
      Dictionary<string, BackofficeUser> users = await Session.LoadAsync<BackofficeUser>(userIds);

      // create revision objects
      foreach (T item in items)
      {
        Revision<T> revision = new()
        {
          Model = item,
          ModelId = item.Id,
          ChangeVector = Session.Advanced.GetChangeVectorFor(item),
          Date = item.LastModifiedDate
        };

        if (!item.LastModifiedById.IsNullOrEmpty() && users.TryGetValue(item.LastModifiedById, out BackofficeUser user) && user != null)
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

      return new ListResult<Revision<T>>(revisions, totalResults, pageNumber, pageSize);
    }


    /// <summary>
    /// Get revision list for an entity
    /// </summary>
    public async Task<ListResult<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity
    {
      ListResult<Revision<T>> items = await GetPagedWithModel<T>(id, pageNumber, pageSize);
      return items.MapTo(model => new Revision()
      {
        ChangeVector = model.ChangeVector,
        Date = model.Date,
        ModelId = model.ModelId,
        User = model.User
      });
    }
  }


  public interface IRevisionsApi
  {
    /// <summary>
    /// Get revision list including models for an entity
    /// </summary>
    Task<ListResult<Revision<T>>> GetPagedWithModel<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity;

    /// <summary>
    /// Get revision list for an entity
    /// </summary>
    Task<ListResult<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity;
  }
}
