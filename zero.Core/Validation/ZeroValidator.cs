using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace zero.Validation;

public class ZeroValidator<TInterface> : ZeroValidator<TInterface, TInterface>
{
}

public abstract class ZeroValidator<TInterface, TImplementation> : AbstractValidator<TImplementation>, IValidator<TInterface> where TImplementation : TInterface
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
