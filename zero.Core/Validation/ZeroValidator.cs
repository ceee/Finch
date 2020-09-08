using System;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Api;
using Raven.Client.Documents;

namespace zero.Core.Validation
{
  public abstract class ZeroValidator<TInterface, TImplementation> : AbstractValidator<TImplementation>, IValidator<TInterface> where TImplementation : TInterface
  {
    protected IBackofficeStore Store { get; private set; }

    protected IDocumentStore Raven { get; private set; }


    public ZeroValidator(IBackofficeStore store)
    {
      Store = store;
      Raven = store.Raven;
    }

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
}
