using System.Linq.Expressions;
using Finch.Numbers;

namespace Finch.Raven;

public class RavenNumberStoreDbProvider : IFinchNumberStoreDbProvider
{
  protected IRavenOperations Ops { get; set; }
  
  
  public RavenNumberStoreDbProvider(IRavenOperations ops)
  {
    Ops = ops;
  }


  public Task<T> Load<T>(string id, CancellationToken ct = default) where T : FinchEntity, new() =>
    Ops.Load<T>(id);


  public Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity =>
    Ops.Session.Query<T>().FirstOrDefaultAsync(expression, ct);
  

  public async Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) 
    where T : FinchEntity =>
    await Ops.Session.Query<T>().Where(expression).ToListAsync(ct);

  
  public Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : FinchEntity, new() =>
    Ops.Create(model);


  public Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : FinchEntity, new() =>
    Ops.Update(model);


  public Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : FinchEntity, new() =>
    Ops.Delete(model);
}