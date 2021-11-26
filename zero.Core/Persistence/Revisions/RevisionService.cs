namespace zero.Persistence;

public class RevisionService : IRevisionService
{
  protected IZeroStore Store { get; set; }

  public RevisionService(IZeroStore store)
  {
    Store = store;
  }


  /// <summary>
  /// Get revision list for an entity
  /// </summary>
  public async Task<Paged<Revision<T>>> GetPagedWithModel<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity
  {
    IZeroDocumentSession session = Store.Session();

    // get paged revisions
    List<T> items = await session.Advanced.Revisions.GetForAsync<T>(id, (pageNumber - 1) * pageSize, pageSize);
    long totalResults = await session.Advanced.Revisions.GetCountForAsync(id);

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
    Dictionary<string, ZeroUser> users = await session.LoadAsync<ZeroUser>(userIds);

    // create revision objects
    foreach (T item in items)
    {
      Revision<T> revision = new()
      {
        Model = item,
        ModelId = item.Id,
        ChangeVector = session.Advanced.GetChangeVectorFor(item),
        Date = item.LastModifiedDate
      };

      if (!item.LastModifiedById.IsNullOrEmpty() && users.TryGetValue(item.LastModifiedById, out ZeroUser user) && user != null)
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

    return new Paged<Revision<T>>(revisions, totalResults, pageNumber, pageSize);
  }


  /// <summary>
  /// Get revision list for an entity
  /// </summary>
  public async Task<Paged<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity
  {
    Paged<Revision<T>> items = await GetPagedWithModel<T>(id, pageNumber, pageSize);
    return items.MapTo(model => new Revision()
    {
      ChangeVector = model.ChangeVector,
      Date = model.Date,
      ModelId = model.ModelId,
      User = model.User
    });
  }
}


public interface IRevisionService
{
  /// <summary>
  /// Get revision list including models for an entity
  /// </summary>
  Task<Paged<Revision<T>>> GetPagedWithModel<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity;

  /// <summary>
  /// Get revision list for an entity
  /// </summary>
  Task<Paged<Revision>> GetPaged<T>(string id, int pageNumber = 1, int pageSize = 10) where T : ZeroEntity;
}
