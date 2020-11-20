using FluentValidation;
using zero.Core.Entities;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class MediaValidator : ZeroValidator<IMedia>
  {
    public MediaValidator()
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.IsActive).Equal(true);
    }
  }
}
