using System;
using System.Collections.Generic;
using System.Data;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceStack.OrmLite;
using zero.Context;
using zero.Models;
using zero.Utils;
using zero.Validation;

namespace zero.Sqlite;

public partial class DbOperations : IDbOperations
{
  protected IZeroContext Context { get; private set; }

  protected FlavorOptions Flavors { get; private set; }

  protected IServiceProvider Services { get; private set; }

  protected IDbConnection Db { get; private set; }

  protected ILogger<IDbOperations> Logger { get; private set; }

  
  public DbOperations(StoreContext context, IDbConnection db, ILogger<IDbOperations> logger)
  {
    Context = context.Context;
    Services = context.Services;
    Flavors = context.Options.For<FlavorOptions>();
    Db = db;
    Logger = logger;
  }


  /// <inheritdoc />
  public bool EnsureTableExists<T>() where T : ZeroIdEntity
  {
    return Db.CreateTableIfNotExists<T>();
  }


  /// <inheritdoc />
  public Task<string> GenerateId<T>(T model) where T : ZeroIdEntity
  {
    return Task.FromResult(IdGenerator.Create(12));
  }


  /// <inheritdoc />
  public T AutoSetIds<T>(T model)
  {
    return IdGenerator.Autofill(model);
  }


  /// <inheritdoc />
  public T PrepareForSave<T>(T model) where T : ZeroIdEntity
  {
    // set IDs
    AutoSetIds(model);

    if (model is not ZeroEntity zeroModel)
    {
      return model;
    }

    // set default properties
    if (zeroModel.CreatedDate == default)
    {
      zeroModel.CreatedDate = DateTimeOffset.Now;
    }

    // update name alias and last modified
    zeroModel.Alias = Safenames.Alias(zeroModel.Name);
    zeroModel.LastModifiedDate = DateTimeOffset.Now;
    zeroModel.Hash ??= IdGenerator.Create();

    return model;
  }


  /// <inheritdoc />
  public async Task<ValidationResult> Validate<T>(T model) where T : ZeroIdEntity, new()
  {
    IZeroMergedValidator<T> validator = Services.GetService<IZeroMergedValidator<T>>();

    if (validator == null)
    {
      return new();
    }

    return await validator.ValidateAsync(model);
  }


  /// <inheritdoc />
  public virtual T WhenActive<T>(T model) where T : ZeroIdEntity, new()
  {
    return model; // TODO should we really use this? I tried to get data in a custom backend but couldn't because of this
    //return model != null && (model is not ZeroEntity || (model as ZeroEntity).IsActive) && (model is not ISupportsSoftDelete || !(model as ISupportsSoftDelete).IsDeleted) ? model : default;
  }
}


public interface IDbOperations
{
  /// <summary>
  /// Create a table if not existing
  /// </summary>
  bool EnsureTableExists<T>() where T : ZeroIdEntity;

  /// <summary>
  /// Generate model Id by using configured document store conventions
  /// </summary>
  Task<string> GenerateId<T>(T model) where T : ZeroIdEntity;

  /// <summary>
  /// Do only return the model when it is set to active or inactive entities are included with IncludeInactive()
  /// </summary>
  T WhenActive<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Validates an entity
  /// </summary>
  Task<ValidationResult> Validate<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Generate values for all properties (incl. nested) which contain the [GenerateId] attribute
  /// </summary>
  T AutoSetIds<T>(T model);

  /// <summary>
  /// Automatically fill base properties of a ZeroEntity
  /// </summary>
  T PrepareForSave<T>(T model) where T : ZeroIdEntity;

  /// <summary>
  /// Get an entity by Id
  /// </summary>
  Task<T> Load<T>(string id, string changeVector = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by ids
  /// </summary>
  Task<Dictionary<string, T>> Load<T>(IEnumerable<string> ids) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by ids
  /// </summary>
  Task<List<T>> LoadAsList<T>(IEnumerable<string> ids) where T : ZeroIdEntity, new();

  /// <summary>
  /// Check if any items exist in this collection (with optional query)
  /// </summary>
  Task<bool> Any<T>(Expression<Func<T, bool>> querySelector = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by query
  /// </summary>
  Task<List<T>> Load<T>(Expression<Func<T, bool>> querySelector) where T : ZeroIdEntity, new();

  /// <summary>
  /// Find entity by query
  /// </summary>
  Task<T> Find<T>(Expression<Func<T, bool>> querySelector) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by sql query
  /// </summary>
  Task<List<T>> LoadBySql<T>(Func<SqlExpression<T>, SqlExpression<T>> querySelector) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get all entities from this collection. 
  /// Warning: Don't use this method for large collections. Stream the results instead.
  /// </summary>
  Task<List<T>> LoadAll<T>() where T : ZeroIdEntity, new();

  /// <summary>
  /// Creates an entity with an optional validator
  /// </summary>
  Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Updates an entity with an optional validator
  /// </summary>
  Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Checks if an entity exists (via ID) and creates or updates it afterwards accordingly
  /// </summary>
  Task<Result<T>> CreateOrUpdate<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Updates sorting of all items in a collection based on the given enumerable
  /// </summary>
  Task Sort<T>(IEnumerable<string> ids) where T : ZeroEntity, new();

  /// <summary>
  /// Deletes an entity
  /// </summary>
  Task<Result<T>> Delete<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Deletes an entity
  /// </summary>
  Task<Result<T>> Delete<T>(string id) where T : ZeroIdEntity, new();
}