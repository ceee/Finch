using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client;

namespace zero.Raven;

public partial class RavenOperations : IRavenOperations
{
  /// <inheritdoc />
  public virtual Task<Result<T>> Delete<T>(T model) where T : ZeroIdEntity, new()
    => Delete<T>(model.Id);


  /// <inheritdoc />
  public virtual async Task<Result<T>> Delete<T>(string id) where T : ZeroIdEntity, new()
  {
    T model = await Load<T>(id);
    
    if (model == null)
    {
      return Result<T>.Fail("@errors.ondelete.idnotfound");
    }

    InterceptorInstruction<T> instruction = Interceptors.ForDelete(model);

    if (InterceptorBlocker == null && !await instruction.Start(this))
    {
      return instruction.Result;
    }

    if (model is ISupportsSoftDelete softDeleteModel)
    {
      softDeleteModel.IsDeleted = true;
    }
    else
    {
      Session.Delete<T>(model);
    }

    await Session.SaveChangesAsync();
    if (InterceptorBlocker == null)
    {
      await instruction.Complete();
    }
    await Session.SaveChangesAsync();

    return Result<T>.Success();
  }


  /// <inheritdoc />
  public virtual async Task Purge<T>(string querySuffix = null, Parameters parameters = null) where T : ZeroIdEntity, new()
  {
    var collectionName = Store.Raven.Conventions.FindCollectionName(typeof(T));
    var operationQuery = new DeleteByQueryOperation(new IndexQuery()
    {
      Query = $"from {collectionName} c {querySuffix ?? String.Empty}",
      QueryParameters = parameters
    }, new QueryOperationOptions { AllowStale = true });

    Operation operation = await Store.Raven.GetOperationExecutor().SendAsync(operationQuery);
    await operation.WaitForCompletionAsync();
  }
}