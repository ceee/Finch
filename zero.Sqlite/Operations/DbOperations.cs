using System;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
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


  public DbOperations(StoreContext context)
  {
    Context = context.Context;
    Services = context.Services;
    Flavors = context.Options.For<FlavorOptions>();
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
}


public interface IDbOperations
{
  /// <summary>
  /// Generate model Id by using configured document store conventions
  /// </summary>
  Task<string> GenerateId<T>(T model) where T : ZeroIdEntity;

  /// <summary>
  /// Generate values for all properties (incl. nested) which contain the [GenerateId] attribute
  /// </summary>
  T AutoSetIds<T>(T model);

  /// <summary>
  /// Automatically fill base properties of a ZeroEntity
  /// </summary>
  T PrepareForSave<T>(T model) where T : ZeroIdEntity;
}