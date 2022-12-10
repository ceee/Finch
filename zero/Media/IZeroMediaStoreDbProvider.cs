using System.Linq.Expressions;

namespace zero.Media;

public interface IZeroMediaStoreDbProvider
{
  Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity;
  
  Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity;

  Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new();
  
  Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new();
  
  Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new();
}