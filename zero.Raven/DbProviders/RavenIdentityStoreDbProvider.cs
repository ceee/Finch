using System.Linq.Expressions;
using Raven.Client.Documents;
using zero.Identity;

namespace zero.Raven;

public class RavenIdentityStoreDbProvider : IZeroIdentityStoreDbProvider
{
  protected IRavenOperations Ops { get; set; }
  
  
  public RavenIdentityStoreDbProvider(IRavenOperations ops)
  {
    Ops = ops;
  }


  public Task<T> Load<T>(string id, CancellationToken ct = default) where T : ZeroEntity, new() =>
    Ops.Load<T>(id);


  public Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity =>
    Ops.Session.Query<T>().FirstOrDefaultAsync(expression, ct);
  

  public async Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) 
    where T : ZeroEntity =>
    await Ops.Session.Query<T>().Where(expression).ToListAsync(ct);

  
  public Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new() =>
    Ops.Create(model);


  public Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new() =>
    Ops.Update(model);


  public Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new() =>
    Ops.Delete(model);
}