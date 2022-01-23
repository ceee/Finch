using FluentValidation;
using FluentValidation.Results;
using System.Collections.Concurrent;

namespace zero.Validation;

public class ZeroMergedValidator<T> : IZeroMergedValidator<T>
{
  protected IEnumerable<IValidator<T>> Validators { get; private set; }

  ConcurrentDictionary<Type, IEnumerable<IValidator<T>>> TypeCache = new();


  public ZeroMergedValidator(IEnumerable<IValidator<T>> validators)
  {
    Validators = validators;
  }


  /// <inheritdoc />
  public async Task<ValidationResult> ValidateAsync(T model)
  {
    ValidationResult result = new();

    foreach (IValidator<T> validator in ResolveFor(model))
    {
      ValidationResult innerResult = await validator.ValidateAsync(model);
      result.Errors.AddRange(innerResult.Errors);
    }

    return result;
  }


  /// <inheritdoc />
  public IEnumerable<IValidator<T>> ResolveFor(T model)
  {
    Type type = typeof(T);

    if (!TypeCache.TryGetValue(type, out IEnumerable<IValidator<T>> validators))
    {
      validators = Validators.Where(validator => CanHandle(validator, type)).ToList();
      TypeCache.TryAdd(type, validators);
    }

    return validators;
  }


  /// <summary>
  /// Checks whether a certain validator can handle the give type
  /// </summary>
  bool CanHandle(IValidator<T> validator, Type modelType)
  {
    Type validatorType = validator.GetType();
    Type typeToFind = typeof(ZeroValidator<,>);

    Type findValidatorBase(Type type)
    {
      if (type.BaseType == null)
      {
        return null;
      }

      if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeToFind)
      {
        return type.BaseType;
      }

      return findValidatorBase(type.BaseType);
    }

    Type zeroValidatorType = findValidatorBase(validatorType);

    if (zeroValidatorType == null)
    {
      return false;
    }

    Type implementationType = zeroValidatorType.GenericTypeArguments[1];

    return implementationType.IsAssignableFrom(modelType);
  }
}


public interface IZeroMergedValidator<T>
{
  /// <summary>
  /// Get all validators which can run for the given model
  /// </summary>
  IEnumerable<IValidator<T>> ResolveFor(T model);

  /// <summary>
  /// Validates a model by using all registered ZeroValidators for this entity type
  /// </summary>
  Task<ValidationResult> ValidateAsync(T model);
}