using System.Linq.Expressions;

namespace Finch.Media;

public interface IFinchMediaStoreDbProvider
{
  Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity;
  
  Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity;

  Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
  
  Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
  
  Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
}


public class EmptyFinchMediaStoreDbProvider : IFinchMediaStoreDbProvider
{
    public Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : FinchEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : FinchEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity
    {
        throw new NotImplementedException();
    }

    public Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : FinchEntity, new()
    {
        throw new NotImplementedException();
    }
}