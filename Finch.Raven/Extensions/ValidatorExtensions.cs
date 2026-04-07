using FluentValidation;

namespace Finch.Raven;

public static class ValidatorExtensions
{
  /// <summary>
  /// Check if this value is unique within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> Unique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IRavenOperations ops) 
    where T : FinchIdEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      bool any = await ops.Session.Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(FinchIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyPath.ToPascalCaseId(), value)
        .AnyAsync(cancellation);

      return !any;
    }).WithMessage("@errors.forms.not_unique");
  }


  /// <summary>
  /// Check if this value is unique within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> Unique<T, TProperty, TCollection>(this IRuleBuilder<T, TProperty> ruleBuilder, IRavenOperations ops) 
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      bool any = await ops.Session.Advanced.AsyncDocumentQuery<TCollection>()
        .WhereEquals(context.PropertyPath.ToPascalCaseId(), value)
        .AnyAsync(cancellation);

      return !any;
    }).WithMessage("@errors.forms.not_unique");
  }


  /// <summary>
  /// Check if this value is at least set once to the expected value within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> ExpectAnyUnique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IRavenOperations ops, TProperty expectedValue) 
    where T : FinchIdEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      return await ops.Session.Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(FinchIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyPath.ToPascalCaseId(), expectedValue)
        .AnyAsync(cancellation);
    }).WithMessage("@errors.forms.not_unique_alone");
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T>(this IRuleBuilder<T, string> ruleBuilder, IRavenOperations ops) 
    where T : FinchIdEntity
  {
    return ruleBuilder.Exists<T, T>(ops);
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T, TCollection>(this IRuleBuilder<T, string> ruleBuilder, IRavenOperations ops) 
    where TCollection : FinchIdEntity
  {
    return ruleBuilder.MustAsync(async (entity, id, context, cancellation) =>
    {
      if (id.IsNullOrWhiteSpace())
      {
        return true;
      }

      return await ops.Session.Query<TCollection>().AnyAsync(x => x.Id == id);
    }).WithMessage("@errors.forms.reference_notfound");
  }
}
