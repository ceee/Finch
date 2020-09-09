using FluentValidation;
using zero.Core.Entities;

namespace zero.Core.Validation
{
  public class MediaFolderValidator : ZeroValidator<IMediaFolder>
  {
    public MediaFolderValidator()
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.IsActive).Equal(true);
    }
  }
}