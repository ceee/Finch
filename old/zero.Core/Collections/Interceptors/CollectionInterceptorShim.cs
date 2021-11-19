using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public sealed class CollectionInterceptorShim<T> : CollectionInterceptor<T> where T : ZeroIdEntity
  {
    ICollectionInterceptor _base;

    public CollectionInterceptorShim(ICollectionInterceptor baseInterceptor)
    {
      _base = baseInterceptor;
    }

    InterceptorResult<T> Result(InterceptorResult<ZeroIdEntity> result)
    {
      return result == null ? null : new InterceptorResult<T>()
      {
        InterceptorHash = result.InterceptorHash,
        Parameters = result.Parameters,
        Prevent = result.Prevent,
        Result = result.Result != null ? EntityResult<T>.From(result.Result, result.Result.Model as T) : null
      };
    }

    /// <inheritdoc />
    public override bool CanRun(InterceptorParameters args, T model) => _base.CanRun(args, model);

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Creating(InterceptorParameters args, T model) => Result(await _base.Creating(args, model));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Updating(InterceptorParameters args, T model) => Result(await _base.Updating(args, model));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Saving(InterceptorParameters args, T model) => Result(await _base.Saving(args, model));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Deleting(InterceptorParameters args, T model) => Result(await _base.Deleting(args, model));

    /// <inheritdoc />
    public override Task Created(InterceptorParameters args, T model) => _base.Created(args, model);

    /// <inheritdoc />
    public override Task Updated(InterceptorParameters args, T model) => _base.Updated(args, model);

    /// <inheritdoc />
    public override Task Saved(InterceptorParameters args, T model) => _base.Saved(args, model);

    /// <inheritdoc />
    public override Task Deleted(InterceptorParameters args, T model) => _base.Deleted(args, model);
  }
}
