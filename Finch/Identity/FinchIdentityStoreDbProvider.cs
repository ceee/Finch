using System.Linq.Expressions;

namespace Finch.Identity;

public interface IFinchIdentityStoreDbProvider
{
  Task<T> Load<T>(string id, CancellationToken ct = default) where T : FinchEntity, new();

  Task<T> Find<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity;
  
  Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression, CancellationToken ct = default) where T : FinchEntity;

  Task<Result<T>> Create<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
  
  Task<Result<T>> Update<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
  
  Task<Result<T>> Delete<T>(T model, CancellationToken ct = default) where T : FinchEntity, new();
}