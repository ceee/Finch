using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class MediaFolderValidator : ZeroValidator<MediaFolder>
  {
    public MediaFolderValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.IsActive).Equal(true);
      RuleFor(x => x.ParentId).Exists(store);
    }
  }
}