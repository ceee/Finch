using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using Sparrow.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class RevisionsApi : IRevisionsApi
  {
    protected IZeroOptions Options { get; set; }

    protected IZeroStore Store { get; set; }

    protected IUserApi UserApi { get; set; }


    public RevisionsApi(IZeroOptions options, IZeroStore store, IUserApi userApi)
    {
      Options = options;
      Store = store;
      UserApi = userApi;
    }


    /// <summary>
    /// Get revision list for an entity
    /// </summary>
    public async Task<ListResult<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 10, bool includeContent = false) where T : IZeroEntity
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      // get paged revisions
      List<T> items = await session.Advanced.Revisions.GetForAsync<T>(id, (pageNumber - 1) * pageSize, pageSize);
      double totalResults = items.Count;

      // get total results in case items count equals page size
      if (pageSize <= items.Count || pageNumber > 1)
      {
        GetRevisionsCommand command = new GetRevisionsCommand(id, 0, 1, true);
        session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
        command.Result.Results.Parent.TryGet("TotalResults", out totalResults);
      }

      // add creation for last page
      if (!items.Any() && pageNumber >= (int)Math.Ceiling((decimal)totalResults / pageSize))
      {
        T currentRevision = await session.LoadAsync<T>(id);
        if (currentRevision != null)
        {
          items.Add(currentRevision);
        }
      }

      List<Revision> revisions = new List<Revision>();

      // load affected users as the revisions could have been edited by other users too
      string[] userIds = items.Select(x => x.LastModifiedById).Distinct().ToArray();
      Dictionary<string, IBackofficeUser> users = await UserApi.GetByIds(userIds);

      // create revision objects
      foreach (T item in items)
      {
        Revision revision = new Revision()
        {
          ChangeVector = session.Advanced.GetChangeVectorFor(item),
          Date = item.LastModifiedDate,
          Json = includeContent ? JsonConvert.SerializeObject(item) : null
        };

        if (!item.LastModifiedById.IsNullOrEmpty() && users.TryGetValue(item.LastModifiedById, out IBackofficeUser user) && user != null)
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

      return new ListResult<Revision>(revisions, Convert.ToInt64(totalResults), pageNumber, pageSize);
    }
  }


  public interface IRevisionsApi
  {
    /// <summary>
    /// Get revision list (without content JSON) for an entity
    /// </summary>
    Task<ListResult<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 10, bool includeContent = false) where T : IZeroEntity;
  }
}
