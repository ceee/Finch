using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class MediaFolderValidator : ZeroValidator<IMediaFolder>
  {
    public MediaFolderValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.IsActive).Equal(true);
      RuleFor(x => x.ParentId).Exists(store);
    }
  }
}