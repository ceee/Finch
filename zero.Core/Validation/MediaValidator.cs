using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Validation
{
  public class MediaValidator : ZeroValidator<IMedia>
  {
    public MediaValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.IsActive).Equal(true);
    }
  }
}