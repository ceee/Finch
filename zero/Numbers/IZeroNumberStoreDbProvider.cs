using System.Linq.Expressions;
using zero.Numbers;

namespace zero.Numbers;

public interface IZeroNumberStoreDbProvider
{
  Task<T> Load<T>(string id, CancellationToken ct = default) where T : ZeroEntity, new();

  Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity;
  
  Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity;

  Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new();
  
  Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new();
  
  Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new();
}


public class EmptyZeroNumberStoreDbProvider : IZeroNumberStoreDbProvider
{
    public Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity
    {
        throw new NotImplementedException();
    }

    public Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : ZeroEntity
    {
        throw new NotImplementedException();
    }

    public Task<T> Load<T>(string id, CancellationToken ct = default) where T : ZeroEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : ZeroEntity, new()
    {
        throw new NotImplementedException();
    }
}