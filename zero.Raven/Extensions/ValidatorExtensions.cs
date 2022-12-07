using FluentValidation;
using Raven.Client.Documents;
using System.Globalization;
using zero.Extensions;
using zero.Models;
using zero.Raven;

namespace zero.Raven;

public static class ValidatorExtensions
{
  /// <summary>
  /// Check if this value is unique within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> Unique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IZeroStore store) where T : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      bool any = await store.Session().Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(ZeroIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyName.ToPascalCaseId(), value)
        .AnyAsync(cancellation);

      return !any;
    }).WithMessage("@errors.forms.not_unique");
  }


  /// <summary>
  /// Check if this value is at least set once to the expected value within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> ExpectAnyUnique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IZeroStore store, TProperty expectedValue) where T : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      return await store.Session().Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(ZeroIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyName.ToPascalCaseId(), expectedValue)
        .AnyAsync(cancellation);
    }).WithMessage("@errors.forms.not_unique_alone");
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced (appId = shared for shareable entities or appId = current)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T>(this IRuleBuilder<T, string> ruleBuilder, IZeroStore store) where T : ZeroEntity
  {
    return ruleBuilder.Exists<T, T>(store);
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced (appId = shared for shareable entities or appId = current)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T, TReference>(this IRuleBuilder<T, string> ruleBuilder, IZeroStore store) where T : ZeroEntity where TReference : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, id, context, cancellation) =>
    {
      if (id.IsNullOrWhiteSpace())
      {
        return true;
      }

      return await store.Session().Query<TReference>().AnyAsync(x => x.Id == id);
    }).WithMessage("@errors.forms.reference_notfound");
  }
}
