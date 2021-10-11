using FluentValidation;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class MediaFolderValidator : ZeroValidator<MediaFolder>
  {
    public MediaFolderValidator(IZeroDocumentSession session)
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.IsActive).Equal(true);
      RuleFor(x => x.ParentId).Exists(session);
    }
  }
}