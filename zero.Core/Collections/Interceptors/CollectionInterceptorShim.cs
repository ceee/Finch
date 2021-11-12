using System;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public sealed class CollectionInterceptorShim<T> : CollectionInterceptor<T> where T : ZeroEntity
  {
    ICollectionInterceptor _base;

    public CollectionInterceptorShim(ICollectionInterceptor baseInterceptor)
    {
      _base = baseInterceptor;
    }

    TParams Args<TParams>(Parameters args, Action<TParams> modify = null) where TParams : CollectionInterceptor<ZeroEntity>.Parameters, new()
    {
      TParams parameters = new TParams()
      {
        Context = args.Context,
        Properties = args.Properties,
        Session = args.Session,
        Store = args.Store,
        Collection = null, //args.Collection
        //Validator = args.Validator
      };

      modify?.Invoke(parameters);
      return parameters;
    }

    TParams Args<TParams>(ParametersWithModel args, Action<TParams> modify = null) where TParams : CollectionInterceptor<ZeroEntity>.ParametersWithModel, new()
    {
      TParams parameters = Args((Parameters)args, modify);
      parameters.Model = args.Model;
      return parameters;
    }

    InterceptorResult<T> Result(InterceptorResult<ZeroEntity> result)
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
    public override bool CanRun(Parameters args)
    {
      if (args is ParametersWithModel)
      {
        return _base.CanRun(Args<CollectionInterceptor<ZeroEntity>.ParametersWithModel>(args as ParametersWithModel));
      }
      return _base.CanRun(Args<CollectionInterceptor<ZeroEntity>.Parameters>(args));
    }

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Creating(CreateParameters args) => Result(await _base.Creating(Args<CollectionInterceptor<ZeroEntity>.CreateParameters>(args)));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Updating(UpdateParameters args) => Result(await _base.Updating(Args<CollectionInterceptor<ZeroEntity>.UpdateParameters>(args, x => x.Id = args.Id)));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Saving(SaveParameters args) => Result(await _base.Saving(Args<CollectionInterceptor<ZeroEntity>.SaveParameters>(args, x => { x.Id = args.Id; x.IsUpdate = args.IsUpdate; })));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Deleting(DeleteParameters args) => Result(await _base.Deleting(Args<CollectionInterceptor<ZeroEntity>.DeleteParameters>(args, x => x.Id = args.Id)));

    /// <inheritdoc />
    public override async Task<InterceptorResult<T>> Purging(PurgeParameters args) => Result(await _base.Purging(Args<CollectionInterceptor<ZeroEntity>.PurgeParameters>(args)));

    /// <inheritdoc />
    public override Task Created(CreateParameters args) => _base.Created(Args<CollectionInterceptor<ZeroEntity>.CreateParameters>(args));

    /// <inheritdoc />
    public override Task Updated(UpdateParameters args) => _base.Updated(Args<CollectionInterceptor<ZeroEntity>.UpdateParameters>(args, x => x.Id = args.Id));

    /// <inheritdoc />
    public override Task Saved(SaveParameters args) => _base.Saved(Args<CollectionInterceptor<ZeroEntity>.SaveParameters>(args, x => { x.Id = args.Id; x.IsUpdate = args.IsUpdate; }));

    /// <inheritdoc />
    public override Task Deleted(DeleteParameters args) => _base.Deleted(Args<CollectionInterceptor<ZeroEntity>.DeleteParameters>(args, x => x.Id = args.Id));

    /// <inheritdoc />
    public override Task Purged(PurgeParameters args) => _base.Purged(Args<CollectionInterceptor<ZeroEntity>.PurgeParameters>(args));
  }
}
