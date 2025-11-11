
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceStack.OrmLite;
using zero.Models;

namespace zero.Sqlite;

public partial class DbOperations : IDbOperations
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
      Logger.LogWarning("Could not delete entity (model is null) for type {type}", typeof(T));
      return Result<T>.Fail("@errors.ondelete.idnotfound");
    }

    if (model is ISupportsSoftDelete softDeleteModel)
    {
      softDeleteModel.IsDeleted = true;
    }
    else
    {
      await Db.DeleteByIdAsync<T>(model.Id);
    }

    Logger.LogInformation("{id} ({type}) successfully deleted", typeof(T), model.Id);

    return Result<T>.Success();
  }
}