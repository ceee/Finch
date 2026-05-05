using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using Mixtape.Extensions;
using Mixtape.Models;

namespace Mixtape.Sqlite;

public partial class DbOperations : IDbOperations
{
  /// <inheritdoc />
  public virtual async Task<T> Load<T>(string id, string changeVector = null) where T : MixtapeIdEntity, new()
  {
    if (id.IsNullOrWhiteSpace())
    {
      return null;
    }
    if (!changeVector.IsNullOrEmpty())
    {
      //return WhenActive(await GetRevision(changeVector)); // TODO
    }

    return WhenActive(await Db.SingleByIdAsync<T>(id));
  }


  /// <inheritdoc />
  public virtual async Task<Dictionary<string, T>> Load<T>(IEnumerable<string> ids) where T : MixtapeIdEntity, new()
  {
    ids = ids.Distinct().ToArray();

    List<T> models = await Db.SelectByIdsAsync<T>(ids);
    Dictionary<string, T> result = new();

    foreach (string id in ids)
    {
      T model = models.FirstOrDefault(x => x.Id == id);
      result.Add(id, WhenActive(model));
    }

    return result;
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> LoadAsList<T>(IEnumerable<string> ids) where T : MixtapeIdEntity, new()
  {
    ids = ids.Distinct().ToArray();

    List<T> models = await Db.SelectByIdsAsync<T>(ids);
    List<T> result = [];

    foreach (string id in ids)
    {
      T model = models.FirstOrDefault(x => x.Id == id);
      if (WhenActive(model) != null)
      {
        result.Add(model);
      }
    }

    return result;
  }


  /// <inheritdoc />
  public virtual async Task<bool> Any<T>(Expression<Func<T, bool>> querySelector = null) where T : MixtapeIdEntity, new()
  {
    return await Db.ExistsAsync(querySelector ?? (x => true));
  }

  
  /// <inheritdoc />
  public virtual async Task<List<T>> Load<T>(Expression<Func<T, bool>> querySelector) where T : MixtapeIdEntity, new()
  {
    return await Db.SelectAsync(querySelector ?? (x => true));
  }
  
  
  /// <inheritdoc />
  public virtual async Task<T> Find<T>(Expression<Func<T, bool>> querySelector) where T : MixtapeIdEntity, new()
  {
    return await Db.SingleAsync(querySelector);
  }
  
  
  /// <inheritdoc />
  public virtual async Task<List<T>> LoadBySql<T>(Func<SqlExpression<T>, SqlExpression<T>> querySelector) where T : MixtapeIdEntity, new()
  {
    return await Db.SelectAsync(querySelector(Db.From<T>()));
  }

  
  /// <inheritdoc />
  public virtual async Task<List<T>> LoadAll<T>() where T : MixtapeIdEntity, new()
  {
    return await Db.SelectAsync<T>(x => true);
  }
}