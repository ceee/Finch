using FluentValidation;
using zero.Core.Entities;

namespace zero.Core.Validation
{
  public class MediaFolderValidator : AbstractValidator<MediaFolder>
  {
    public MediaFolderValidator()
    {
      RuleFor(x => x.Name).Length(2, 80);
    }
  }
}
