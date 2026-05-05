using System.Linq.Expressions;
using Mixtape.Media;

namespace Mixtape.Raven;

public class RavenMediaStoreDbProvider : IMixtapeMediaStoreDbProvider
{
  protected IRavenOperations Ops { get; set; }
  
  
  public RavenMediaStoreDbProvider(IRavenOperations ops)
  {
    Ops = ops;
  }
  
  
  public Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : MixtapeEntity =>
    Ops.Session.Query<T>().FirstOrDefaultAsync(expression, ct);
  

  public async Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) 
    where T : MixtapeEntity =>
    await Ops.Session.Query<T>().Where(expression).ToListAsync(ct);

  
  public Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : MixtapeEntity, new() =>
    Ops.Create(model);


  public Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : MixtapeEntity, new() =>
    Ops.Update(model);


  public Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : MixtapeEntity, new() =>
    Ops.Delete(model);
}