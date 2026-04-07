using System.Linq.Expressions;
using Finch.Numbers;

namespace Finch.Numbers;

public interface IFinchNumberStoreDbProvider
{
  Task<T> Load<T>(string id, CancellationToken ct = default) where T : FinchEntity, new();

  Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity;
  
  Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity;

  Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
  
  Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
  
  Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
}


public class EmptyFinchNumberStoreDbProvider : IFinchNumberStoreDbProvider
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

    public Task<T> Load<T>(string id, CancellationToken ct = default) where T : FinchEntity, new()
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : FinchEntity, new()
    {
        throw new NotImplementedException();
    }
}