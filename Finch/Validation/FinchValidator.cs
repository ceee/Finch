using FluentValidation;
using FluentValidation.Results;

namespace Finch.Validation;

public class FinchValidator<TInterface> : FinchValidator<TInterface, TInterface>
{
}

public abstract class FinchValidator<TInterface, TImplementation> : AbstractValidator<TImplementation>, IValidator<TInterface> where TImplementation : TInterface
{
  public ValidationResult Validate(TInterface instance)
  {
    if (!(instance is TImplementation))
    {
      throw new ArgumentException($"Parameter {nameof(instance)} has to be of type {typeof(TImplementation)}.");
    }
    return base.Validate((TImplementation)instance);
  }

  public Task<ValidationResult> ValidateAsync(TInterface instance, CancellationToken cancellation = default)
  {
    if (!(instance is TImplementation))
    {
      throw new ArgumentException($"Parameter {nameof(instance)} has to be of type {typeof(TImplementation)}.");
    }
    return base.ValidateAsync((TImplementation)instance, cancellation);
  }
}